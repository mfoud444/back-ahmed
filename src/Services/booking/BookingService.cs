using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.BookingDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Services.booking
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly WorkshopRepository _workshopRepository;
        private readonly PaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        public BookingService(
            BookingRepository bookingRepository,
            WorkshopRepository workshopRepository,
            IMapper mapper
        )
        {
            _bookingRepository = bookingRepository;
            _workshopRepository = workshopRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingReadDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<BookingReadDto> GetByIdAsync(Guid id, Guid userId, string userRole)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            if (userRole != UserRole.Admin.ToString() && booking.UserId != userId)
            {
                throw CustomException.Forbidden($"Not allowed to access booking with id: {id}");
            }
            return _mapper.Map<Booking, BookingReadDto>(booking);
        }

        public async Task<List<BookingReadDto>> GetByUserIdAsync(Guid userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound(
                    $"Bookings associated with userId: {userId} not found"
                );
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<List<BookingReadDto>> GetByStatusAsync(string status)
        {
            var bookings = await _bookingRepository.GetByStatusAsync(status);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings with status: {status} not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<List<BookingReadDto>> GetByUserIdAndStatusAsync(
            Guid userId,
            string status
        )
        {
            var bookings = await _bookingRepository.GetByUserIdAndStatusAsync(userId, status);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings with status: {status} not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<List<BookingReadDto>> GetWithPaginationAsync(
            PaginationOptions paginationOptions
        )
        {
            var bookings = await _bookingRepository.GetWithPaginationAsync(paginationOptions);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<List<BookingReadDto>> GetByUserIdWithPaginationAsync(
            PaginationOptions paginationOptions
        )
        {
            var bookings = await _bookingRepository.GetByUserIdWithPaginationAsync(
                paginationOptions
            );
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<BookingReadDto> CreateAsync(BookingCreateDto booking, Guid userId)
        {
            //1. check if the workshop is found
            var workshop = await _workshopRepository.GetByIdAsync(booking.WorkshopId);
            if (workshop == null)
            {
                throw CustomException.NotFound($"Workshp with id: {booking.WorkshopId} not found");
            }
            //2. check if the workshop isn't available
            if (!workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //3. check if the user already enrolled in this workshop
            bool isFound = await _bookingRepository.GetByUserIdAndWorkshopIdAsync(
                userId,
                booking.WorkshopId
            );
            if (isFound)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //4. check if the user enrolled in other workshop at the same time
            var workshops = await _workshopRepository.GetAllAsync();
            var foundWorkshop = workshops.FirstOrDefault(w =>
                (w.StartTime == workshop.StartTime && w.EndTime == workshop.EndTime)
                || (w.StartTime < workshop.StartTime && w.EndTime > workshop.StartTime)
                || (w.StartTime < workshop.EndTime && w.EndTime > workshop.EndTime)
            );
            var isFound2 = false;
            if (foundWorkshop != null)
            {
                isFound2 = await _bookingRepository.GetByUserIdAndWorkshopIdAsync(
                    userId,
                    foundWorkshop.Id
                );
            }
            if (isFound2)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //create booking
            var mappedBooking = _mapper.Map<BookingCreateDto, Booking>(booking);
            mappedBooking.UserId = userId;
            mappedBooking.Status = Status.Pending;
            var createdBooking = await _bookingRepository.CreateAsync(mappedBooking);
            return _mapper.Map<Booking, BookingReadDto>(createdBooking);
        }

        //after payment
        public async Task<BookingReadDto> ConfirmAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            //1. check if the workshop isn't available
            if (!booking.Workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid confirming");
            }
            //2. check if the booking status isn't pending
            if (booking.Status.ToString() != Status.Pending.ToString())
            {
                throw CustomException.BadRequest($"Invalid confirming");
            }
            //3. check if the user doesn't pay
            //var payment =

            //confirm booking
            booking.Status = Status.Confirmed;
            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return _mapper.Map<Booking, BookingReadDto>(updatedBooking);
        }

        //after workshop becomes unavailable
        public async Task<List<BookingReadDto>> RejectAsync(Guid workshopId)
        {
            var workshop = await _workshopRepository.GetByIdAsync(workshopId);
            //1. check if the workshop is found
            if (workshop == null)
            {
                throw CustomException.NotFound($"Workshp with id: {workshopId} not found");
            }
            //2. check if the workshop is available
            if (workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid regecting");
            }
            var bookings = await _bookingRepository.GetByWorkshopIdAndStatusAsync(
                workshopId,
                Status.Pending
            );
            //3. check if there is booking with Pending Status
            if (bookings == null)
            {
                throw CustomException.BadRequest($"Invalid regecting");
            }
            foreach (var booking in bookings)
            {
                //reject booking
                booking.Status = Status.Rejected;
                var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<BookingReadDto> CancelAsync(Guid id, Guid userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            //1. check if the booking belongs to the user
            if (booking.UserId != userId)
            {
                throw CustomException.BadRequest($"Invalid canceling");
            }
            //2. check if the workshop is available
            if (!booking.Workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid canceling");
            }
            //3. check if the booking status isn't pending
            if (booking.Status.ToString() != Status.Pending.ToString())
            {
                throw CustomException.BadRequest($"Invalid canceling");
            }
            //4. check if the user pay
            //var payment =

            //Cancel booking
            booking.Status = Status.Canceled;
            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return _mapper.Map<Booking, BookingReadDto>(updatedBooking);
        }
    }
}

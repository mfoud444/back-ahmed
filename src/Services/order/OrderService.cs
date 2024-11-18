using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Services.order
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;

        private readonly ArtworkRepository _artworkRepository;

        public OrderService(
            OrderRepository OrderRepository,
            IMapper mapper,
            ArtworkRepository artworkRepository
        )
        {
            _orderRepository = OrderRepository;
            _mapper = mapper;
            _artworkRepository = artworkRepository;
        }

        //-----------------------------------------------------

        // Retrieves all orders (Only Admin)
        public async Task<List<OrderReadDto>> GetAllAsync()
        {
            var OrderList = await _orderRepository.GetAllAsync();
            if (OrderList.Count == 0)
            {
                throw CustomException.NotFound($"Orders not found");
            }
            return _mapper.Map<List<Order>, List<OrderReadDto>>(OrderList);
        }

        // Retrieves all orders
        public async Task<List<OrderReadDto>> GetAllAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid order ID");
            }
            var orders = await _orderRepository.GetOrdersByUserIdAsync(id);
            if (orders == null || !orders.Any())
            {
                throw CustomException.NotFound($"No orders found for user with id: {id}");
            }
            return _mapper.Map<List<Order>, List<OrderReadDto>>(orders);
        }

        //-----------------------------------------------------

        // Creates a new order
        public async Task<OrderReadDto> CreateOneAsync(Guid userId, OrderCreateDto createDto)
        {
            // Validate the createDto object
            if (
                createDto == null
                || createDto.OrderDetails == null
                || !createDto.OrderDetails.Any()
            )
            {
                throw CustomException.BadRequest(
                    "Invalid order data or no artworks provided in the order."
                );
            }

            decimal totalAmount = 0; // Initialize total amount

            // Process each artwork in the order
            foreach (var orderDetail in createDto.OrderDetails)
            {
                // Fetch the artwork by its ID
                var artwork = await _artworkRepository.GetByIdAsync(orderDetail.ArtworkId); // Using ArtworkId

                // Validate if the artwork exists
                if (artwork == null)
                {
                    throw CustomException.NotFound(
                        $"Artwork with ID: {orderDetail.ArtworkId} not found."
                    );
                }

                // Check if there is enough stock for the requested quantity
                if (artwork.Quantity < orderDetail.Quantity)
                {
                    throw CustomException.BadRequest(
                        $"Artwork {artwork.Title} does not have enough stock. Requested: {orderDetail.Quantity}, Available: {artwork.Quantity}."
                    );
                }

                // Reduce artwork stock
                artwork.Quantity -= orderDetail.Quantity;

                // Update the artwork quantity in the repository
                await _artworkRepository.UpdateOneAsync(artwork);

                // Calculate the total amount for this order detail
                decimal detailAmount = artwork.Price * orderDetail.Quantity;

                // Add this amount to the total amount
                totalAmount += detailAmount;
            }

            // Set the order creation time
            createDto.CreatedAt = DateTime.UtcNow;

            var newOrder = _mapper.Map<OrderCreateDto, Order>(createDto);

            // Set the user ID on the new order
            newOrder.UserId = userId;
            newOrder.TotalAmount = totalAmount;

            // Save the order to the repository
            var createdOrder = await _orderRepository.CreateOneAsync(newOrder);

            // Return the created order as a DTO
            return _mapper.Map<Order, OrderReadDto>(createdOrder);
        }

        //-----------------------------------------------------

        // Retrieves a order by their ID (Only Admin)
        public async Task<OrderReadDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid order ID");
            }
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            return _mapper.Map<Order, OrderReadDto>(foundOrder);
        }

        // Retrieves a order by their ID
        public async Task<OrderReadDto> GetByIdAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid order ID");
            }
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            if (foundOrder.UserId != userId)
            {
                throw CustomException.Forbidden("You are not authorized to view this order.");
            }

            return _mapper.Map<Order, OrderReadDto>(foundOrder);
        }

        //-----------------------------------------------------

        // Deletes a order by their ID
        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            return await _orderRepository.DeleteOneAsync(foundOrder);
        }

        // Updates a order by their ID
        public async Task<bool> UpdateOneAsync(Guid id, OrderUpdateDto updateDto)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }

            // Map the update DTO to the existing Order entity
            _mapper.Map(updateDto, foundOrder);
            return await _orderRepository.UpdateOneAsync(foundOrder);
        }

        public async Task<List<OrderReadDto>> GetOrdersByPage(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("Page Size should be greater than 0.");
            }

            if (paginationOptions.PageNumber < 0)
            {
                throw CustomException.BadRequest("Page Number should be 0 or greater.");
            }
            var OrderList = await _orderRepository.GetAllAsync(paginationOptions);
            if (OrderList == null || !OrderList.Any())
            {
                throw CustomException.NotFound("Orders not found");
            }
            return _mapper.Map<List<Order>, List<OrderReadDto>>(OrderList);
        }

        public async Task<List<OrderReadDto>> SortOrdersByDate()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders.Count == 0)
            {
                throw CustomException.NotFound("Orders not found");
            }
            var sortedOrders=orders.OrderBy(x => x.CreatedAt).ToList();
            return _mapper.Map<List<Order>, List<OrderReadDto>>(sortedOrders);
        }
    }
}

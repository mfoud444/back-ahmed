using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.UserDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Services.user
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            UserRepository UserRepository,
            IMapper mapper,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _userRepository = UserRepository;
            _mapper = mapper;
        }

        // Retrieves all users
        public async Task<List<UserReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("PageSize should be greater than 0.");
            }

            if (paginationOptions.PageNumber <= 0)
            {
                throw CustomException.BadRequest("PageNumber should be greater than 0.");
            }

            var UserList = await _userRepository.GetAllAsync(paginationOptions);
            if (!UserList.Any() || UserList == null)
            {
                throw CustomException.NotFound($"Users not found");
            }
            return _mapper.Map<List<User>, List<UserReadDto>>(UserList);
        }

        // Gets the total count of users
        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userRepository.GetCountAsync();
        }

        // Creates a new user
        public async Task<UserReadDto> CreateOneAsync(UserCreateDto createDto)
        {
            if (createDto == null)
            {
                throw CustomException.BadRequest("User data cannot be null.");
            }
            if (
                createDto
                    .Role.ToString()
                    .Equals(UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase)
            )
            {
                throw CustomException.UnAuthorized(
                    "Only admin users can create other admin accounts."
                );
            }
            // Hash password before saving to the database
            PasswordUtils.HashPassword(
                createDto.Password,
                out string hashedPassword,
                out byte[] salt
            );
            var user = _mapper.Map<UserCreateDto, User>(createDto);
            user.Password = hashedPassword;
            user.Salt = salt;

            var UserCreated = await _userRepository.CreateOneAsync(user);
            if (UserCreated == null)
            {
                throw CustomException.BadRequest("Failed to create user.");
            }
            return _mapper.Map<User, UserReadDto>(UserCreated);
        }

        //-----------------------------------------------------

        // Retrieves a user by their ID (Admin,customer,artist)
        public async Task<UserReadDto> GetByIdAsync(Guid userId)
        {
            var foundUser = await _userRepository.GetByIdAsync(userId);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with id: {userId} not found");
            }
            return _mapper.Map<User, UserReadDto>(foundUser);
        }

        //-----------------------------------------------------

        // Deletes a user by their ID
        public async Task<bool> DeleteOneAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid user ID");
            }
            var foundUser = await _userRepository.GetByIdAsync(id);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with ID {id} not found.");
            }
            var DeletedUser = await _userRepository.DeleteOneAsync(foundUser);

            // Check if the delete was successful
            if (!DeletedUser)
            {
                throw CustomException.BadRequest("Failed to delete user.");
            }

            return DeletedUser;
        }

        //-----------------------------------------------------

        // Updates a user by their ID
        public async Task<bool> UpdateOneAsync(Guid id, UserUpdateDto updateDto)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid user ID");
            }
            if (updateDto == null)
            {
                throw CustomException.BadRequest("Update data cannot be null");
            }

            var foundUser = await _userRepository.GetByIdAsync(id);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with ID {id} not found.");
            }

            // Map the update DTO to the existing User entity
            _mapper.Map(updateDto, foundUser);

            // Hash password before saving to the database if it's provided
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                PasswordUtils.HashPassword(
                    updateDto.Password,
                    out string hashedPassword,
                    out byte[] salt
                );
                foundUser.Password = hashedPassword;
                foundUser.Salt = salt;
            }

            var updatedUser = await _userRepository.UpdateOneAsync(foundUser);

            // Check if the update was successful
            if (!updatedUser)
            {
                throw CustomException.BadRequest("Failed to update user.");
            }

            return updatedUser;
        }

        //-----------------------------------------------------

        // Retrieves a user by their email
        public async Task<UserReadDto> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw CustomException.BadRequest("Email is required");
            }
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw CustomException.NotFound($"User with email {email} not found.");
            }
            return _mapper.Map<User, UserReadDto>(user);
        }

        // Retrieves a user by their phone number
        public async Task<UserReadDto> GetByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw CustomException.BadRequest("Phone Number is required");
            }

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw CustomException.NotFound("User not found.");
            }
            return _mapper.Map<User, UserReadDto>(user);
            ;
        }

        // Signs in a user with their credentials
        public async Task<string> SignInAsync(UserSigninDto signinDto)
        {
            if (signinDto == null)
            {
                throw CustomException.BadRequest("User data cannot be null.");
            }

            var foundUser = await _userRepository.GetByEmailAsync(signinDto.Email);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with E-mail: {signinDto.Email} not found.");
            }

            // Verify the password
            bool isMatched = PasswordUtils.VerifyPassword(
                signinDto.Password,
                foundUser.Password,
                foundUser.Salt
            );

            if (!isMatched)
            {
                throw CustomException.UnAuthorized($"Unauthorized access.");
            }

            var TokenUtil = new TokenUtils(_configuration);
            var token = TokenUtil.GenerateToken(foundUser);

            if (string.IsNullOrEmpty(token))
            {
                throw CustomException.UnAuthorized("Failed to generate token.");
            }

            return token;
        }
    }
}

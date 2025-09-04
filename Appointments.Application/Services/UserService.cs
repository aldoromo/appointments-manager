using Appointments.Application.DTOs;
using Appointments.Application.Exceptions;
using Appointments.Application.Interfaces;
using Appointments.Infrastructure.Entities;
using Appointments.Infrastructure.Interfaces;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id) ?? throw new Exception("User not found");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _repository.GetByUsernameAsync(username);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<int> CreateAsync(UserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.UserId;
        }

        public async Task UpdateAsync(UserDto dto, int userId)
        {
            var entity = await _repository.GetByIdAsync(dto.UserId);
            if (entity == null) throw new NotFoundException($"User {dto.UserId} not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            if (entity.UserId != userId && user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("You cannot update this user");

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var entity = await _repository.GetByIdAsync(userId);
            if (entity == null) throw new NotFoundException($"Appointment {userId} not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            if (entity.UserId != userId && user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("You cannot delete this appointment");

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

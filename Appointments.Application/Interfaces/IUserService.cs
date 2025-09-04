using Appointments.Application.DTOs;


namespace Appointments.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<int> CreateAsync(UserDto dto);        
        Task UpdateAsync(UserDto dto, int userId);
        Task DeleteAsync(int userId);
    }
}

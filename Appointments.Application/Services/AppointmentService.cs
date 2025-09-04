using System.Collections.Generic;
using Appointments.Application.DTOs;
using Appointments.Application.Exceptions;
using Appointments.Application.Interfaces;
using Appointments.Infrastructure.Entities;
using Appointments.Infrastructure.Interfaces;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync(string orderBy=null)
        {
            var list = await _repository.ListAsync(orderBy);
            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Appointment {id} not found");

            return _mapper.Map<AppointmentDto>(entity);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(int userId, string orderBy=null)
        {
            var list = await _repository.FindByUserIdAsync(userId);
             

            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            var list = await _repository.FindByDateRangeAsync(from, to);
            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<int> CreateAsync(AppointmentDto dto, int userId)
        {
            dto.UserId = userId;
            var entity = _mapper.Map<Appointment>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.AppointmentId;
        }

        public async Task UpdateAsync(AppointmentDto dto, int userId)
        {
            var entity = await _repository.GetByIdAsync(dto.AppointmentId);
            if (entity == null) throw new NotFoundException($"Appointment {dto.AppointmentId} not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            if (entity.UserId != userId && user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("You cannot update this appointment");

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ApproveAsync(int id, int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("Only managers can approve appointments");

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Appointment {id} not found");
            if(entity.Status == AppointmentStatus.Canceled) throw new UnauthorizedAccessException($"Appointment {id} is already cancelled");

            entity.Status = AppointmentStatus.Approved;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelAsync(int id, int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("Only managers can cancel appointments");

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Appointment {id} not found");

            entity.Status = AppointmentStatus.Canceled;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int appointmentId, int userId)
        {
            var entity = await _repository.GetByIdAsync(appointmentId);
            if (entity == null) throw new NotFoundException($"Appointment {appointmentId} not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            if (entity.UserId != userId && user.Role != UserRole.Manager)
                throw new UnauthorizedAccessException("You cannot delete this appointment");
            
            if (user.Role == UserRole.Manager) 
            { 
                if(entity.Status != AppointmentStatus.Canceled)
                    throw new UnauthorizedAccessException("You have to cancel first this appointment");
            }

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

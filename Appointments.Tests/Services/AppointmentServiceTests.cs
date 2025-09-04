using Appointments.Application.DTOs;
using Appointments.Application.Services;
using Appointments.Infrastructure.Entities;
using Appointments.Infrastructure.Interfaces;
using AutoMapper;
using Moq;


namespace Appointments.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly IMapper _mapper;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Appointment, AppointmentDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _unitOfWorkMock.SetupGet(u => u.Appointments).Returns(_appointmentRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Users).Returns(_userRepoMock.Object);

            _service = new AppointmentService(_appointmentRepoMock.Object, _unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldAssignUserId()
        {
            // arrange
            var dto = new AppointmentDto { Title = "Test Meeting" };
            int userId = 1;

            // act
            await _service.CreateAsync(dto, userId);

            // assert
            _appointmentRepoMock.Verify(r => r.AddAsync(It.Is<Appointment>(a => a.UserId == userId)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenUserNotOwnerOrManager()
        {
            // arrange
            var dto = new AppointmentDto { AppointmentId = 5, Title = "Update Test" };
            int userId = 10; // trying to update but not owner
            var appointment = new Appointment { AppointmentId = 5, UserId = 20, Title = "Old Title" };
            var user = new User { UserId = userId, Role = (int)UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateAsync(dto, userId));
        }

        [Fact]
        public async Task UpdateAsync_ShouldAllow_WhenUserIsOwner()
        {
            // arrange
            var dto = new AppointmentDto { AppointmentId = 5, Title = "Updated" };
            int userId = 10;
            var appointment = new Appointment { AppointmentId = 5, UserId = userId, Title = "Old" };
            var user = new User { UserId = userId, Role = (int)UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await _service.UpdateAsync(dto, userId);

            // assert
            _appointmentRepoMock.Verify(r => r.Update(It.IsAny<Appointment>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ApproveAsync_ShouldThrow_WhenUserIsNotManager()
        {
            // arrange
            int appointmentId = 1;
            int userId = 10;
            var appointment = new Appointment { AppointmentId = appointmentId, Status = (int)AppointmentStatus.Pending };
            var user = new User { UserId = userId, Role = (int)UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.ApproveAsync(appointmentId, userId));
        }

        [Fact]
        public async Task ApproveAsync_ShouldUpdateStatus_WhenUserIsManager()
        {
            // arrange
            int appointmentId = 1;
            int userId = 99;
            var appointment = new Appointment { AppointmentId = appointmentId, Status = (int)AppointmentStatus.Pending };
            var user = new User { UserId = userId, Role = UserRole.Manager };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await _service.ApproveAsync(appointmentId, userId);

            // assert
            Assert.Equal(AppointmentStatus.Approved, appointment.Status);
            _appointmentRepoMock.Verify(r => r.Update(It.IsAny<Appointment>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenUserIsNotManager()
        {
            // arrange
            int appointmentId = 2;
            int userId = 15;
            var appointment = new Appointment {UserId = appointmentId, Status = AppointmentStatus.Pending };
            var user = new User { UserId = userId, Role = UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.CancelAsync(appointmentId, userId));
        }

        [Fact]
        public async Task CancelAsync_ShouldUpdateStatus_WhenUserIsManager()
        {
            // arrange
            int appointmentId = 2;
            int userId = 99;
            var appointment = new Appointment { AppointmentId = appointmentId, Status = (int)AppointmentStatus.Pending };
            var user = new User { UserId = userId, Role = UserRole.Manager };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await _service.CancelAsync(appointmentId, userId);

            // assert
            Assert.Equal(AppointmentStatus.Canceled, appointment.Status);
            _appointmentRepoMock.Verify(r => r.Update(It.IsAny<Appointment>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenUserIsNotOwnerOrManager()
        {
            // arrange
            int appointmentId = 3;
            int userId = 15;
            var appointment = new Appointment { AppointmentId = appointmentId, UserId = 20 };
            var user = new User { UserId = userId, Role = (int)UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteAsync(appointmentId, userId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldAllow_WhenUserIsOwner()
        {
            // arrange
            int appointmentId = 3;
            int userId = 15;
            var appointment = new Appointment { AppointmentId = appointmentId, UserId = userId };
            var user = new User { UserId = userId, Role = (int)UserRole.User };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await _service.DeleteAsync(appointmentId, userId);

            // assert
            _appointmentRepoMock.Verify(r => r.Delete(It.Is<Appointment>(a => a.AppointmentId == appointmentId)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldAllow_WhenUserIsManager()
        {
            // arrange
            int appointmentId = 3;
            int userId = 99;
            var appointment = new Appointment { AppointmentId = appointmentId, UserId = 123 };
            var user = new User { UserId = userId, Role = UserRole.Manager };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await Assert.ThrowsAnyAsync<Exception>(async ()=> await _service.DeleteAsync(appointmentId, userId));

             
        }


        [Fact]
        public async Task Cancell_ShouldntCancellApprovedAppointments()
        {

            // arrange
            int appointmentId = 3;
            int userId = 99;
            var appointment = new Appointment { AppointmentId = appointmentId, UserId = 123 , Status= AppointmentStatus.Approved};
            var user = new User { UserId = userId, Role = UserRole.Manager };

            _appointmentRepoMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // act
            await Assert.ThrowsAnyAsync<Exception>(async () => await _service.CancelAsync(appointmentId, userId));

        }
    }
}
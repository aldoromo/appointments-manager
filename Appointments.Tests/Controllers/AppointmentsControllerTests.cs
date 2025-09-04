using Appointments.Application.DTOs;
using Appointments.Application.Interfaces;
using Appointments.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Appointments.Tests
{
    public class AppointmentsControllerTests
    {
        private readonly Mock<IAppointmentService> _serviceMock;
        private readonly AppointmentsController _controller;

        public AppointmentsControllerTests()
        {
            _serviceMock = new Mock<IAppointmentService>();
            _controller = new AppointmentsController(_serviceMock.Object);

            // Simular HttpContext con UserId en Items
            var httpContext = new DefaultHttpContext();
            httpContext.Items["UserId"] = 42; // user simulado
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }
 

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenAppointmentExists()
        {
            // arrange
            int appointmentId = 5;
            _serviceMock.Setup(s => s.GetByIdAsync(appointmentId))
                .ReturnsAsync(new AppointmentDto { AppointmentId = appointmentId, Title = "Test" });

            // act
            var result = await _controller.GetById(appointmentId);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<AppointmentDto>(okResult.Value);
            Assert.Equal(appointmentId, dto.AppointmentId);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenAppointmentDoesNotExist()
        {
            // arrange
            int appointmentId = 99;
            _serviceMock.Setup(s => s.GetByIdAsync(appointmentId)).ReturnsAsync((AppointmentDto)null);

            // act
            var result = await _controller.GetById(appointmentId);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            // arrange
            var dto = new AppointmentDto { AppointmentId = 1, Title = "New Meeting" };
            _serviceMock.Setup(s => s.CreateAsync(dto, It.IsAny<int>())).ReturnsAsync(1);

            // act
            var result = await _controller.Create(dto);

            // assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdResult.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenIdsMatch()
        {
            // arrange
            int appointmentId = 10;
            var dto = new AppointmentDto { AppointmentId = appointmentId, Title = "Update" };

            // act
            var result = await _controller.Update(appointmentId, dto);

            // assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(dto, 42), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsMismatch()
        {
            // arrange
            int appointmentId = 10;
            var dto = new AppointmentDto { AppointmentId = 20, Title = "Mismatch" };

            // act
            var result = await _controller.Update(appointmentId, dto);

            // assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("AppointmentId mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // arrange
            int appointmentId = 15;

            // act
            var result = await _controller.Delete(appointmentId);

            // assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(appointmentId, 42), Times.Once);
        }

        [Fact]
        public async Task Approve_ShouldReturnNoContent()
        {
            // arrange
            int appointmentId = 100;

            // act
            var result = await _controller.Approve(appointmentId);

            // assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.ApproveAsync(appointmentId, 42), Times.Once);
        }

        [Fact]
        public async Task Cancel_ShouldReturnNoContent()
        {
            // arrange
            int appointmentId = 200;

            // act
            var result = await _controller.Cancel(appointmentId);

            // assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.CancelAsync(appointmentId, 42), Times.Once);
        }

    }
}
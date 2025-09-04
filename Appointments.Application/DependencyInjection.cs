using Appointments.Application.DTOs;
using Appointments.Application.Interfaces;
using Appointments.Application.Services;
using Appointments.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IUserService, UserService>();

            // AutoMapper profile
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : "Unknown"))
                .ReverseMap();

            });

            return services;
        }
    }
}

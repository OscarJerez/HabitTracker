using HabitTracker.Application.Common.Behaviors;
using FluentValidation;
using HabitTracker.Application.Features.Habits.Commands;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Queries;
using HabitTracker.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using HabitTracker.Application.Commands;
using HabitTracker.Application.Common;
using HabitTracker.Application;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Application.Features.Habits.Commands.Validators;



namespace HabitTracker.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IHabitRepository, HabitRepository>();

            // MediatR 
            services.AddMediatR(typeof(GetHabitsForUserQuery).Assembly);

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<UpdateHabitCommandValidator>();
            // AutoMapper registration
            //services.AddAutoMapper(typeof(MappingProfile).Assembly);
            
            // Pipeline behaviors 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}

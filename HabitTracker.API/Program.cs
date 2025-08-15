using System.Text;
using FluentValidation;
using HabitTracker.API.Extensions;
using HabitTracker.Application.Common;
using HabitTracker.Application.Features.Habits.Commands.Validators;
using HabitTracker.Application.Interfaces.Identity;
using HabitTracker.Application.Interfaces.Security;
using HabitTracker.Infrastructure.Data;
using HabitTracker.Infrastructure.Identity;
using HabitTracker.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using HabitTracker.Application.Features.Habits.Queries;

namespace HabitTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
            );

            // Controllers
            builder.Services.AddControllers();

            // Swagger + JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Oscar´s Habit Tracker API",
                    Version = "v1",
                    Description = "Clean Architecture + CQRS + MediatR + AutoMapper"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {space} your_token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Application services
            DependencyInjection.AddApplicationServices(builder.Services);

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateHabitCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<DeleteHabitCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetHabitsForUserQueryValidator>();

            // JWT Authentication
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            var signingKeyBytes = Encoding.UTF8.GetBytes(jwtSettingsSection["SigningKey"]!);

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettingsSection["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSettingsSection["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Infrastructure
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IUserService, UserService>();

            // CORS (optional)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            var app = builder.Build();

            // HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Habit Tracker API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

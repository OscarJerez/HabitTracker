using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using AutoMapper; 
using HabitTracker.Application.Common; 
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Queries;
using HabitTracker.Domain.Entities;

namespace HabitTracker.Tests
{
    public class GetHabitsForUserQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnHabits_ForGivenUserId()
        {
            // Arrange
            Guid testUserId = Guid.NewGuid();

            List<Habit> habits = new List<Habit>
            {
                new Habit { Id = Guid.NewGuid(), Title = "Morning Workout", Description = "Go to the gym before work", UserId = testUserId },
                new Habit { Id = Guid.NewGuid(), Title = "Read a Book", Description = "Read 30 minutes daily", UserId = Guid.NewGuid() } // other user
            };

            // Fake repository seeded with test data
            FakeHabitRepository fakeRepository = new FakeHabitRepository(habits);

            // Configure AutoMapper for the test scope (reuse your application MappingProfile)
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            // Handler now requires both repository and mapper
            GetHabitsForUserQueryHandler handler = new GetHabitsForUserQueryHandler(fakeRepository, mapper);

            // Query for habits by user id
            GetHabitsForUserQuery query = new GetHabitsForUserQuery(testUserId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.All(result.Data!, h => Assert.Equal(testUserId, h.UserId));
        }
    }
}

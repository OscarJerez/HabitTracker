using AutoMapper;
using HabitTracker.Application.Commands.Handlers;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Domain.Entities;

namespace HabitTracker.Tests
{
    public class CreateHabitCommandHandlerTests
    {
        [Fact] // This attribute indicates a test method for xUnit
        public async Task Handle_ShouldAddHabit_WhenValidCommandIsGiven()
        {
            // Arrange
            Guid testUserId = Guid.NewGuid();

            // Create DTO
            CreateHabitDto createHabitDto = new CreateHabitDto
            {
                Title = "Drink Water",
                Description = "Daily habit to improve focus",
            };

            // Create command with DTO and user ID
            CreateHabitCommand createHabitCommand = new(createHabitDto, testUserId);

            // Fake repository
            FakeHabitRepository fakeHabitRepository = new FakeHabitRepository(new List<Habit>());

            // Configure AutoMapper for test scope
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<CreateHabitDto, Habit>();
                configuration.CreateMap<Habit, HabitDto>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            // Create handler with both repository and mapper
            CreateHabitCommandHandler handler = new CreateHabitCommandHandler(fakeHabitRepository, mapper);

            // Act
            var result = await handler.Handle(createHabitCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Single(fakeHabitRepository.Habits);
            Assert.Equal("Drink Water", fakeHabitRepository.Habits[0].Title);
            Assert.Equal(testUserId, fakeHabitRepository.Habits[0].UserId);
        }
    }
}

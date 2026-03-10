namespace SurveyBasket.Tests.Polls.Query;

public class GetAllPollsQueryHandlerTests
{
    private readonly Mock<IPollRepository> _pollRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllPollsQueryHandler _handler;

    public GetAllPollsQueryHandlerTests()
    {
        _pollRepositoryMock = new Mock<IPollRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllPollsQueryHandler(
            _pollRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    // Test 1: returns success when polls exist
    [Fact]
    public async Task Handle_WhenPollsExist_ReturnSuccess()
    {
        // Arrange
        var polls = new List<Poll>
        {
            new Poll { PollId = 1, Title = "Poll 1", Description = "Desc 1" },
            new Poll { PollId = 2, Title = "Poll 2", Description = "Desc 2" }
        };

        var pollDTOs = new List<PollResponseDTO>
        {
            new PollResponseDTO { PollId = 1, Title = "Poll 1", Description = "Desc 1" },
            new PollResponseDTO { PollId = 2, Title = "Poll 2", Description = "Desc 2" }
        };

        _pollRepositoryMock
            .Setup(x => x.getAllPollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(polls);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<PollResponseDTO>>(polls))
            .Returns(pollDTOs);

        // Act
        var result = await _handler.Handle(new GetAllPollsQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
        result.Value.First().Title.Should().Be("Poll 1");
    }

    // Test 2: returns empty list when no polls exist
    [Fact]
    public async Task Handle_WhenNoPollsExist_ReturnSuccessWithEmptyList()
    {
        // Arrange
        var emptyPolls = new List<Poll>();
        var emptyDTOs = new List<PollResponseDTO>();

        _pollRepositoryMock
            .Setup(x => x.getAllPollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPolls);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<PollResponseDTO>>(emptyPolls))
            .Returns(emptyDTOs);

        // Act
        var result = await _handler.Handle(new GetAllPollsQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    // Test 3: verify repository was called exactly once
    [Fact]
    public async Task Handle_Always_CallsRepositoryOnce()
    {
        // Arrange
        _pollRepositoryMock
            .Setup(x => x.getAllPollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Poll>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<PollResponseDTO>>(It.IsAny<IEnumerable<Poll>>()))
            .Returns(new List<PollResponseDTO>());

        // Act
        await _handler.Handle(new GetAllPollsQuery(), CancellationToken.None);

        // Assert
        _pollRepositoryMock.Verify(
            x => x.getAllPollsAsync(It.IsAny<CancellationToken>()),
            Times.Once // ← repository must be called exactly once
        );
    }
}
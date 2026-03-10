using SurveyBasket.BLL.Features.Polls.Queries.GetAvilablePolls;

namespace SurveyBasket.Tests.Polls.Query;
public class GetAvaliblePollsQueryHandlerTests
{
    private readonly Mock<IPollRepository> _repositoryMock;
    private readonly GetAvaliblePollsQueryHandler _handler;

    public GetAvaliblePollsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IPollRepository>();
        _handler = new GetAvaliblePollsQueryHandler(
            _repositoryMock.Object
            );
    }

    // retrun list of polls 
    [Fact]
    public async Task Handle_WhenAvailblePolls_ReturnSuccess()
    {
        //Arrenge
        var polls = new List<Poll>
        {
            new Poll{PollId = 1,Title = "Poll 1" , Description = "This is first Poll",isPublished = true, startDate = DateOnly.FromDateTime(DateTime.UtcNow),endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))},
            new Poll{PollId = 2,Title = "Poll 2" , Description = "This is second Poll",isPublished = true, startDate = DateOnly.FromDateTime(DateTime.UtcNow),endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))},
        };

        
        _repositoryMock
            .Setup(x=>x.getAvailblePollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(polls);

        //Act
        var result = await _handler.Handle(new GetAvaliblePollsQuery(),CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
        result.Value.First().Title.Should().Be("Poll 1");
    }
    // return not found
    [Fact]
    public async Task Handle_WhenNoAvailblePolls_ReturnSuccessWithEmtyList()
    {
        //Arrenge

        _repositoryMock
            .Setup(x=>x.getAvailblePollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Poll>());

        // Act 
        var result = await _handler.Handle(new GetAvaliblePollsQuery(),CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
    // call repo once
    [Fact]
    public async Task Handle_Always_CallRepositoryonce()
    {
        //Arrenge
        _repositoryMock
            .Setup(x => x.getAvailblePollsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Poll>());
        //Act
        await _handler.Handle(new GetAvaliblePollsQuery(),CancellationToken.None);

        //assert
        _repositoryMock.Verify(
            x=>x.getAvailblePollsAsync(It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

}

using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Polls.Queries.GetPollById;

namespace SurveyBasket.Tests.Polls.Query;
public class GetPollByIdQueryHandlerTests
{
    private readonly Mock<IPollRepository> _pollRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPollByIdQueryHandler _handler;
    public GetPollByIdQueryHandlerTests()
    {
        _pollRepositoryMock = new Mock<IPollRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPollByIdQueryHandler(
            _pollRepositoryMock.Object,
            _mapperMock.Object
            );
    }
    // if poll  exist
    [Fact]
    public async Task Handle_WhenPollExists_ReturnSuccess()
    {
        // Arrange
        var poll = new Poll
        {
            PollId = 1,
            Title = "Poll 1",
            Description = "This is the first Poll"
        };

        var pollDTO = new PollResponseDTO
        {
            PollId = 1,
            Title = "Poll 1",
            Description = "This is the first Poll"
        };

        _pollRepositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(poll); 

        _mapperMock
            .Setup(x => x.Map<PollResponseDTO>(poll))
            .Returns(pollDTO);

        // Act
        var result = await _handler.Handle(new GetPollByIdQuery(1), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Poll 1"); // ← Value is single object
        result.Value.Description.Should().Be("This is the first Poll");
    }

    // if poll not found
    [Fact]
    public async Task Handle_WhenPollNotFound_ReturnFailureWithEmpty()
    {
        // Arrenge
        Poll? nullPoll = null;
        _pollRepositoryMock
            .Setup(x => x.getPollByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullPoll!);

        //Act
        var result = await _handler.Handle(new GetPollByIdQuery(99), CancellationToken.None);
        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollNotFound);
    }

    //Call repository once
    [Fact]
    public async Task Handle_Alwaye_CallRepositoryOnce()
    {
        //Arrenge 
        Poll? nullPoll = null;
        _pollRepositoryMock
            .Setup(x => x.getPollByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullPoll!);
        // Act 
        await _handler.Handle(new GetPollByIdQuery(1),CancellationToken.None);

        _pollRepositoryMock.Verify(
            x => x.getPollByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

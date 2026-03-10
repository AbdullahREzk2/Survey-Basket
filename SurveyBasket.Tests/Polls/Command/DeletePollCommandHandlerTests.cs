using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Polls.Commands.DeletePoll;

namespace SurveyBasket.Tests.Polls.Command;
public class DeletePollCommandHandlerTests
{
    private readonly Mock<IPollRepository> _repositoryMock;
    private readonly DeletePollCommandHandler _handler;

    public DeletePollCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPollRepository>();
        _handler = new DeletePollCommandHandler(
            _repositoryMock.Object
            );
    }

    // delete success
    [Fact]
    public async Task Handle_whenDeletedSuccess_ReturnSucess()
    {
        //arrenge 
        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Poll());

        _repositoryMock
            .Setup(x => x.DeletePollAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        // act
        var result = await _handler.Handle(new DeletePollCommand(1),CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeTrue();

    }

    // poll Not Found
    [Fact]
    public async Task Handle_whenPollNotFound_ReturnFailure()
    {
        //arrenge 
        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Poll)null!);

        // act
        var result = await _handler.Handle(new DeletePollCommand(1), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollNotFound);
    }

    // delete failed
    [Fact]
    public async Task Handle_whenDeletedFailed_ReturnFailure()
    {
        //arrenge 
        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Poll());

        _repositoryMock
            .Setup(x => x.DeletePollAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        // act
        var result = await _handler.Handle(new DeletePollCommand(1), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollDeletionFailed);

    }

    // hit Repo once
    [Fact]
    public async Task Handle_Always_CheckRepoHitOnce()
    {

        //arrenge 
        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Poll());

        _repositoryMock
            .Setup(x => x.DeletePollAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        // act
        var result = await _handler.Handle(new DeletePollCommand(1), CancellationToken.None);
        //assert
        _repositoryMock.Verify(
            x=>x.DeletePollAsync(1,It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

}

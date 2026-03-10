using MediatR;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Notifications.Command.SendNewPollNotification;
using SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;

namespace SurveyBasket.Tests.Polls.Command;
public class PollPublishToggleCommandHandlerTests
{

    private readonly Mock<IPollRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PollPublishToggleCommandHandler _handler;
    public PollPublishToggleCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPollRepository>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new PollPublishToggleCommandHandler(
            _repositoryMock.Object,
            _mediatorMock.Object
            );
    }

    // Publish success
    [Fact]
    public async Task Handle_whenPublishSuccess_ReturnSuccess()
    {
        // arrenge
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test",
            isPublished=false
        };

        _repositoryMock
            .Setup(x => x.publishToggle(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);    

        //act
        var result = await _handler.Handle(new PollPublishToggleCommand(1), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeTrue();

    }
    // Poll PublicationToggle Failed
    [Fact]
    public async Task Handle_whenPublishFailed_ReturnFailed()
    {
        // arrenge
        _repositoryMock
            .Setup(x=>x.publishToggle(1,It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        // act
        var result = await _handler.Handle(new PollPublishToggleCommand(1),CancellationToken.None);
        // assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollPublicationToggleFailed);
    }
    //hit repo once
    [Fact]
    public async Task Handle_always_HitRepoOnce()
    {

        // arrenge
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test",
            isPublished = false
        };

        _repositoryMock
            .Setup(x => x.publishToggle(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);

        //act
        var result = await _handler.Handle(new PollPublishToggleCommand(1), CancellationToken.None);
        //assert
        _repositoryMock.Verify(
            x=>x.publishToggle(1,It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}

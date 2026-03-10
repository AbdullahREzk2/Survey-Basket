using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Polls.Commands.UpdatePoll;

namespace SurveyBasket.Tests.Polls.Command;
public class UpdatePollCommandHandlerTests
{
    private readonly Mock<IPollRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdatePollCommandHandler _handler;

    public UpdatePollCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPollRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdatePollCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object
            );
    }

    // updated successfuky 
    [Fact]
    public async Task Handle_whenUpdatedValid_ReturnSuccess()
    {
        //Arrenge
        var pollRequest = new PollRequestDTO
        {
            Title = "Test 2"
        };
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test"
        };
        var pollUpdated = new Poll
        {
            PollId = 1,
            Title = "Test 2"
        };
        var pollResponse = new PollResponseDTO
        {

            PollId = 1,
            Title = "Test 2"
        };

        _mapperMock
            .Setup(x => x.Map<Poll>(pollRequest))
            .Returns(pollEntity);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);

        _repositoryMock
            .Setup(x => x.UpdatePollAsync(1, pollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollUpdated);

        _mapperMock
           .Setup(x => x.Map<PollResponseDTO>(pollUpdated))
           .Returns(pollResponse);

        //Act
        var result = await _handler.Handle(new UpdatePollCommand(1, pollRequest),CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Test 2");
    }

    // poll not found
    [Fact]
    public async Task Handle_whenPollNotFound_ReturnFailure()
    {
        //arrenge 
        var pollRequest = new PollRequestDTO
        {
            Title = "Test"
        };
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test"
        };

        _mapperMock
         .Setup(x => x.Map<Poll>(pollRequest))
         .Returns(pollEntity);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Poll)null!);

        //act
        var result = await _handler.Handle(new UpdatePollCommand(99, pollRequest), CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollNotFound);
    }

    // updated failed
    [Fact]
    public async Task Handle_UpdatedFailed_ReturnUpdatedFailed()
    {
        //arrenge 
        var pollRequest = new PollRequestDTO
        {
            Title = "Test"
        };
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test"
        };
        _mapperMock
         .Setup(x => x.Map<Poll>(pollRequest))
         .Returns(pollEntity);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);

        _repositoryMock
            .Setup(x => x.UpdatePollAsync(1, pollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Poll)null!);

        //act
        var result = await _handler.Handle(new UpdatePollCommand(1, pollRequest), CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollUPdateFailed);
    }

    // hit repo once
    [Fact]
    public async Task Handle_Always_hitRepositoryOnce()
    {
        //Arrenge
        var pollRequest = new PollRequestDTO
        {
            Title = "Test 2"
        };
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test"
        };
        var pollUpdated = new Poll
        {
            PollId = 1,
            Title = "Test 2"
        };
        var pollResponse = new PollResponseDTO
        {

            PollId = 1,
            Title = "Test 2"
        };

        _mapperMock
            .Setup(x => x.Map<Poll>(pollRequest))
            .Returns(pollEntity);

        _repositoryMock
            .Setup(x => x.getPollByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);

        _repositoryMock
            .Setup(x => x.UpdatePollAsync(1, pollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollUpdated);

        _mapperMock
           .Setup(x => x.Map<PollResponseDTO>(pollUpdated))
           .Returns(pollResponse);

        //Act
        var result = await _handler.Handle(new UpdatePollCommand(1, pollRequest), CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            x=>x.UpdatePollAsync(1,pollEntity,It.IsAny<CancellationToken>())
            ,Times.Once
            );
    }

}

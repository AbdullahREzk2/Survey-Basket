using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Polls.Commands.AddPoll;

namespace SurveyBasket.Tests.Polls.Command;
public class AddPollCommandHandlerTests
{
    private readonly Mock<IPollRepository> _pollRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddPollCommandHandler _handler;

    public AddPollCommandHandlerTests()
    {
        _pollRepositoryMock = new Mock<IPollRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddPollCommandHandler(
            _pollRepositoryMock.Object,
            _mapperMock.Object
            );
    }
    //when poll created successfuly
    [Fact]
    public async Task Handle_whenValidPoll_ReturnSuccess()
    {
        //arrenge
        var pollEntity = new Poll
        {
            PollId = 1,
            Title = "Test",
            Description = "Test"
        };
        var pollRequest = new PollRequestDTO
        {
            Title = "Test",
            Description = "Test"
        };
        var pollDTO = new PollResponseDTO
        {
            PollId = 1,
            Title = "Test",
            Description = "Test"
        };

        _mapperMock
            .Setup(x => x.Map<Poll>(pollRequest))
            .Returns(pollEntity);

        _pollRepositoryMock
            .Setup(x => x.SearchPollByTitleAsync(pollEntity.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _pollRepositoryMock
            .Setup(x => x.AddPollAsync(pollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pollEntity);

        _mapperMock
            .Setup(x=>x.Map<PollResponseDTO>(pollEntity))
            .Returns(pollDTO);

        //act
        var result = await _handler.Handle(new AddPollCommand(pollRequest),CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Test");

    }

    // when title is exist --> failure
    [Fact]
    public async Task Handle_whenTitleExist_ReturnFailure()
    {
        //arrenge
        var pollEntity = new Poll
        {
            PollId =1,
            Title = "Test"
        };

        var requestPoll = new PollRequestDTO
        {
            Title = "Test"
        };

        _mapperMock
        .Setup(x => x.Map<Poll>(requestPoll))
        .Returns(pollEntity);

        _pollRepositoryMock
            .Setup(x => x.SearchPollByTitleAsync(pollEntity.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // act
        var result = await _handler.Handle(new AddPollCommand(requestPoll),CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollAlreadyExists);

    }

    //when creation failed
    [Fact]
    public async Task Handle_WhenCreationFailed_ReturnFailure()
    {
        // arrenge
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

        _pollRepositoryMock
            .Setup(x => x.SearchPollByTitleAsync(pollEntity.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _pollRepositoryMock.Setup(x => x.AddPollAsync(pollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Poll)null!);
            
        // act 
        var result = await _handler.Handle(new AddPollCommand(pollRequest),CancellationToken.None);
        // assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PollErrors.PollCreationFailed);
    }

    // repositry call once
    [Fact]
    public async Task Handle_Always_RepositoryCallOnce()
    {
        //Arrenge
        var pollRequest = new PollRequestDTO
        {
            Title = "Test"
        };
        var PollEntity = new Poll
        {
            PollId = 1,
            Title = "Test"
        };
        var pollDTO = new PollResponseDTO
        {
            PollId = 1,
            Title = "Test",
            Description = "Test"
        };
        _mapperMock
            .Setup(x => x.Map<Poll>(pollRequest))
            .Returns(PollEntity);

        _pollRepositoryMock
            .Setup(x => x.SearchPollByTitleAsync(PollEntity.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _pollRepositoryMock.Setup(x => x.AddPollAsync(PollEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PollEntity);

        // Act
        await _handler.Handle(new AddPollCommand(pollRequest), CancellationToken.None);

        // Assert
        _pollRepositoryMock.Verify(
            x => x.AddPollAsync(PollEntity,It.IsAny<CancellationToken>()),
            Times.Once // ← repository must be called exactly once
        );
    }

}

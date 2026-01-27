namespace SurveyBasket.DAL.DTO;
public record VotesPerDayDTO(
    DateOnly Date,
    int VoteCount
);

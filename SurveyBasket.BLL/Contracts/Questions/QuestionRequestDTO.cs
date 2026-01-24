namespace SurveyBasket.BLL.Contracts.Questions;
public record QuestionRequestDTO(
     string Content,
     List<string> Answers
    );


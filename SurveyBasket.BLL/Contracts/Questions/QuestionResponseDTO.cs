namespace SurveyBasket.BLL.Contracts.Questions;
public class QuestionResponseDTO
{
    public int questionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool isActive { get; set; }
    public List<AnswerResponseDTO> answers { get; set; } = [];
}


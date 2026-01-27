namespace SurveyBasket.DAL.DTO;
public class QuestionAnswerDTO
{
    public int VoteId { get; set; }
    public string QuestionContent { get; set; } = default!;
   public string AnswerContent { get; set; } = default!;
   
}

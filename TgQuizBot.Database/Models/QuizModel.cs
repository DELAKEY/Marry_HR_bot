namespace TgQuizBot.Database.Models
{
    public class QuizModel : ModelBase
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
    }
}

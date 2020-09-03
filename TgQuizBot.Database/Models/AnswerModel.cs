namespace TgQuizBot.Database.Models
{
    public class AnswerModel : ModelBase
    {
        public virtual int Id { get; set; }
        public virtual int Quest { get; set; }
        public virtual int Intervier { get; set; }
        public virtual string Text { get; set; }
    }
}

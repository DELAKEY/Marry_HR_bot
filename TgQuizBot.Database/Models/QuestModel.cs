namespace TgQuizBot.Database.Models
{
    public class QuestModel : ModelBase
    {
        public virtual int Id { get; set; }
        public virtual long Quiz { get; set; }
        public virtual string Text { get; set; }
        public virtual AnswerEnum AnswerType { get; set; }
        public virtual long Previous { get; set; }
    }
    public enum AnswerEnum
    {
        Text,
        Inn,
        Close,
        EditClose,
        Contact,
        File,
        FileOrText
    }
}

namespace TgQuizBot.Database.Models
{
    public class LangModel : ModelBase
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Wellcome {get;set;}
        public virtual string Emploer { get; set; }
        public virtual string Applicant { get; set; }
    }
}

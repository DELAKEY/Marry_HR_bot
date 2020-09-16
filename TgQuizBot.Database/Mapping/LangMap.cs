using FluentNHibernate.Mapping;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    public class LangMap : ClassMap<LangModel>
    {
        public LangMap()
        {
            Table("langs");
            Id(x => x.Id).Column("id");
            Map(x => x.Code).Column("code");
            Map(x => x.Wellcome).Column("wellcome");
            Map(x => x.Emploer).Column("emploer");
            Map(x => x.Applicant).Column("applicant");
        }
    }
}

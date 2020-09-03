using FluentNHibernate.Mapping;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    public class QuestMap : ClassMap<QuestModel>
    {
        public QuestMap()
        {
            Table("quests");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.Quiz).Column("quiz");
            Map(x => x.Text).Column("text");
            Map(x => x.AnswerType).Column("answertype");
            Map(x => x.Previous).Column("previous");
        }
    }
}

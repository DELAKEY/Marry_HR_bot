using FluentNHibernate.Mapping;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    public class QuizMap : ClassMap<QuizModel>
    {
        public QuizMap() {
            Table("quizes");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();

        }
    }
}

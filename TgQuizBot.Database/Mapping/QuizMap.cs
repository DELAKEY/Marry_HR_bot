using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    public class AnswerMap : ClassMap<AnswerModel>
    {
        public AnswerMap()
        {
            Table("answers");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.Quest).Column("quest");
            Map(x => x.Intervier).Column("intervier");
            Map(x => x.Text).Column("text");
        }
    }
}

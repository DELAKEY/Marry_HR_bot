using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    class IntervierMap : ClassMap<IntervierModel> 
    {
        public IntervierMap()
        {
            Table("interviers");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.User).Column("user");
        }
    }
}

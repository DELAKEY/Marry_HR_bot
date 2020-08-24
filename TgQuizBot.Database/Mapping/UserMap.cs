﻿using FluentNHibernate.Mapping;
using NHibernate.Properties;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Mapping
{
    class UserMap: ClassMap<UserModel>
    {
      public UserMap()
        {
            Table("users");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.TgId).Column("tgid");
            Map(x => x.State).Column("state");
        }
    }
}

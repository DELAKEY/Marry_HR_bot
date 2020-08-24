using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class IntervierRepository : RepositoryBase<IntervierModel>
    {
        public IntervierRepository(ISessionFactory factory) : base(factory) { }
    }
}

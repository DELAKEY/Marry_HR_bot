using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class UserRepository : RepositoryBase<UserModel>
    {
        public UserRepository(ISessionFactory factory) : base(factory) { }
        public UserModel GetByTg(long user)
        {
            using (var session = GetSession())
            {
                var t = new RemovableList<UserModel>(
                    session.QueryOver<UserModel>()
                    .Where(x => x.TgId == user).List());
                if (t.Count == 0)
                {
                    var u = new UserModel()
                    {
                        TgId = user
                    };
                    SaveOrUpdate(u);
                    return u;
                }
                return t[0];
            }
        }
    }
}

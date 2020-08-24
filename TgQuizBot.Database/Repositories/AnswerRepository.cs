using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class AnswerRepository : RepositoryBase<AnswerModel>
    {
        public AnswerRepository(ISessionFactory factory) : base(factory) { }
        public RemovableList<AnswerModel> GetAnswers(long intervier)
        {
            using (var session = GetSession())
            {
                var t = new RemovableList<AnswerModel>(
                    session.QueryOver<AnswerModel>()
                    .Where(x => x.Intervier == intervier).List());

                return t;
            }
        }
    }
}

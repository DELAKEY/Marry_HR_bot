using NHibernate;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class LangRepasitory : RepositoryBase<LangModel>
    {
        public LangRepasitory(ISessionFactory factory) : base(factory) { }
        public LangModel GetByCode(string code)
        {
            using (var session = GetSession())
            {
                var t = new RemovableList<LangModel>(
                    session.QueryOver<LangModel>()
                    .Where(x => (x.Code == code))
                    .List());
                if (t.Count == 0)
                    return null;

                return t[0];
            }
        }
    }
}

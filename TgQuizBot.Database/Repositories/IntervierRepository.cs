using NHibernate;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class IntervierRepository : RepositoryBase<IntervierModel>
    {
        public IntervierRepository(ISessionFactory factory) : base(factory) { }
    }
}

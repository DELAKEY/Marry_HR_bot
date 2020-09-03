using NHibernate;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public class QuestRepository : RepositoryBase<QuestModel>
    {
        public QuestRepository(ISessionFactory factory) : base(factory) { }
        public QuestModel GetFirst(int quiz)
        {
            using (var session = GetSession())
            {
                var t = new RemovableList<QuestModel>(session.QueryOver<QuestModel>().Where(x => (x.Quiz == quiz)&&(x.Previous ==0)).List());
                return t[0];
            }
        }
        public QuestModel GetNext(int quest)
        {
            using (var session = GetSession())
            {
                var t = new RemovableList<QuestModel>(session.QueryOver<QuestModel>().Where(x =>  (x.Previous == quest)).List());
                if (t.Count == 0) return null;
                return t[0];
            }
        }
    }
}

//using Database.Data.Entities.Results;
//using Database.Data.Repositories;
using NHibernate;
using TgQuizBot.Database.Repositories;

namespace TgQuizBot.Database
{
    public class DataBase
    {

        private ISessionFactory _factory;

        public UserRepository Users { get; private set; }
        public QuestRepository Quests { get; private set; }
        public IntervierRepository Interviers { get; private set; }
        public AnswerRepository Answers { get; private set; }
        public LangRepasitory Langs { get; private set; }
        internal DataBase(ISessionFactory factory)
        {
            _factory = factory;

            Users = new UserRepository(factory);
            Quests = new QuestRepository(factory);
            Interviers = new IntervierRepository(factory);
            Answers = new AnswerRepository(factory);
            Langs = new LangRepasitory(factory);
        }
    }
}

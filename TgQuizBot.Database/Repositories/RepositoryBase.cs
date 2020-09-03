using NHibernate;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Database.Repositories
{
    public abstract class RepositoryBase<T> where T : ModelBase
    {
        protected readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory _factory;

        protected ISession GetSession()
        {
            //TODO:Implement caching sessions
            return _factory.OpenSession();
        }

        protected RepositoryBase(ISessionFactory factory)
        {
            _factory = factory;
        }

        protected void Transaction(Action<ISession, ITransaction> onDone, Action<ISession, ITransaction, Exception> onError = null)
        {
            using (var session = GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        onDone?.Invoke(session, transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        //Logger.Error(e, "Transaction fail");
                        transaction.Rollback();
                        onError?.Invoke(session, transaction, e);
                    }
                }
            }
        }

        public IList<T> GetAll()
        {
            using (var session = GetSession())
            {
                return new RemovableList<T>(session.CreateCriteria<T>().List<T>());
            }
        }

        public IList<T> GetTop(int amount)
        {
            using (var session = GetSession())
            {
                return new RemovableList<T>(session.Query<T>().Take(amount).ToList());
            }
        }

        public T GetById(int id)
        {
            using (var session = GetSession())
            {
                return session.Get<T>(id);
            }
        }

        public void SaveOrUpdate(T user)
        {
            Transaction((s, t) => s.SaveOrUpdate(user));
        }
        public void SaveOrUpdate(IEnumerable<T> values)
        {
            Transaction((s, t) =>
            {

                var list = values as RemovableList<T>;
                if (list != null)
                {
                    foreach (var value in values)
                        s.SaveOrUpdate(value);

                    foreach (var value in list.DeletedItems)
                        s.Delete(value);
                }
                else
                {
                    foreach (var value in values)
                    {
                        if (!value.ShouldDelete)
                            s.SaveOrUpdate(value);
                        else
                            s.Delete(value);
                    }
                }
            });
        }

        public void Delete(T user)
        {
            Transaction((s, t) => s.Delete(user));
        }

        public void DeleteAll()
        {

        }
    }
}
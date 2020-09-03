using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Caches.RtMemoryCache;
using NHibernate.Tool.hbm2ddl;

namespace TgQuizBot.Database
{
    public class DataProvider
    {

        public DataProvider(string host, string database, string user, string password)
        {

            var factory = Fluently.Configure()
              .Database(MySQLConfiguration.Standard.ConnectionString(c =>
              {
                  c.Server(host)
                  .Database(database)
                  .Username(user)
                  .Password(password); 
              }))
              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DataProvider>())
              .Mappings(m => m.HbmMappings.AddFromAssemblyOf<DataProvider>())
              .Cache(c => c.ProviderClass<RtMemoryCacheProvider>().UseQueryCache().UseSecondLevelCache())
              .BuildSessionFactory();

            Database = new DataBase(factory);
        }


        public static void CreateDatabase()
        {
            var configuration = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(c =>
                c.Server("82.146.34.243")
                .Username("root2")
                .Password("11223344556677889900")
                .Database("testmap")).ShowSql)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DataProvider>())
                .Mappings(m => m.HbmMappings.AddFromAssemblyOf<DataProvider>())
                .BuildConfiguration();

            var exporter = new SchemaExport(configuration);
            exporter.Execute(true, true, false);

            configuration.BuildSessionFactory();
        }

        public DataBase Database { get; private set; }

    }
}
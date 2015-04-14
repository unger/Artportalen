namespace Artportalen.Sample.Data
{
    using Artportalen.Sample.Data.Mappings;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    public class NHibernateConfiguration
    {
        private static ISessionFactory sessionFactory;

        public static ISession GetSession()
        {
            if (sessionFactory == null)
            {
                sessionFactory = CreateSessionFactory();
            }

            return sessionFactory.OpenSession();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(MsSqlConfiguration
                        .MsSql2008
                        .ConnectionString(System.Configuration.ConfigurationManager.ConnectionStrings["SqlDbConnection"].ConnectionString))
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<SightingDtoMap>())
              .ExposeConfiguration(BuildSchema)
              .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaUpdate(config)
              .Execute(false, true);
        }
    }
}

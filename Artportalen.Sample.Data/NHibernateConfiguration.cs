namespace Artportalen.Sample.Data
{
    using System.IO;

    using Artportalen.Sample.Data.Mappings;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    public class NHibernateConfiguration
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(MsSqlConfiguration
                        .MsSql2012
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
            new SchemaExport(config)
              .Create(false, true);
        }

    }
}

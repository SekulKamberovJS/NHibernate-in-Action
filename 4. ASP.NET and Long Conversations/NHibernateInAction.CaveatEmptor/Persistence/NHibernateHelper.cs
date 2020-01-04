using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NHibernate.Tool.hbm2ddl;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Model;

namespace NHibernateInAction.CaveatEmptor.Persistence
{
    /// <summary> A very simple NHibernate helper class that holds the SessionFactory as a singleton.
    /// 
    /// The only job of this helper class is to give your application code easy
    /// access to the SessionFactory. It initializes the SessionFactory
    /// when it is loaded (static constructor) and you can easily access the
    /// current session.
    /// </summary>
    public class NHibernateHelper
    {
        public static readonly ISessionFactory SessionFactory;

        static NHibernateHelper()
        {
            try
            {
                // Create the initial SessionFactory 
                // from the default configuration file (hibernate.cfg.xml)
                Configuration cfg = new Configuration().Configure();
                CreateMappingsFromAttributesAndAddToConfiguration(cfg);
                DropCreateDatabaseObjects(cfg);
                SessionFactory = cfg.BuildSessionFactory();
                CreateSomeSampleData();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("NHibernate initialization failed", ex);
            }
        }

        private static void CreateSomeSampleData()
        {
            using (ISession session = OpenSession())
            {
                session.SaveOrUpdate(new Item("KName1", "KDescA", 1, 1, DateTime.Now, DateTime.Now));
                session.SaveOrUpdate(new Item("KName2", "KDescB", 1, 1, DateTime.Now, DateTime.Now));
            }
        }

        /// <summary>
        /// Standalone function to drop/create database objects
        /// </summary>
        public static void DropCreateDatabaseObjects()
        {
            Configuration cfg = new Configuration().Configure();
            CreateMappingsFromAttributesAndAddToConfiguration(cfg);
            DropCreateDatabaseObjects(cfg);
        }

        /// <summary>
        /// Drops and creates database objects for a given configuration.
        /// </summary>
        /// <param name="cfg"></param>
        private static void DropCreateDatabaseObjects(Configuration cfg)
        {
            new SchemaExport(cfg).Drop(false, true);
            new SchemaExport(cfg).Create(false, true);
        }

        private static void CreateMappingsFromAttributesAndAddToConfiguration(Configuration cfg)
        {
            HbmSerializer.Default.Validate = true;
            HbmSerializer.Default.HbmDefaultAccess = "field.camelcase";
            using (MemoryStream stream = HbmSerializer.Default.Serialize(Assembly.GetExecutingAssembly()))

            {
                cfg.AddInputStream(stream);
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }
    }
}
using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using Environment=NHibernate.Cfg.Environment;

namespace HelloNHibernate
{
    [Class(Lazy=false)]
    class QuickEmployee
    {
        [Id(Type="int")]
        [Generator(1, Class = "native")] 
        public int id;
        
        [Property(Access="field")] 
        public string name;

        public string SayHello()
        {
            return string.Format(
                "This is {0} saying Hello to NHibernate!",
                name);
        }
    }


    public class ProgramWithAttributes
    {
        static void Main1(string[] args)
        {
            CreateEmployeeAndSaveToDatabase();
        }

        private static void CreateEmployeeAndSaveToDatabase()
        {
            QuickEmployee fred = new QuickEmployee();
            fred.name = "Fred Bloggs";
            using (ISession nhibernateSession = OpenSession())
            {
                nhibernateSession.Save(fred);
                Console.WriteLine("Saved Fred to the database");
            }
        }

        static ISession OpenSession()
        {
            Configuration configuration = ConfigureByCode();
            ISessionFactory factory = configuration.BuildSessionFactory();
            return factory.OpenSession();
        }

        
        static Configuration ConfigureByCode()
        {
            Configuration cfg = new Configuration();
            cfg.Properties[Environment.ConnectionProvider] =
              "NHibernate.Connection.DriverConnectionProvider";
            cfg.Properties[Environment.Dialect] =
              "NHibernate.Dialect.MsSql2000Dialect";
            cfg.Properties[Environment.ConnectionDriver] =
              "NHibernate.Driver.SqlClientDriver";
            cfg.Properties[Environment.ConnectionString] =
              "Data Source=.\\SQLEXPRESS;Initial Catalog=HelloNHibernate;Integrated Security=SSPI";
            cfg.AddAssembly(Assembly.GetCallingAssembly());
            

            HbmSerializer.Default.Validate = true;
            using (MemoryStream stream =
              HbmSerializer.Default.Serialize(
                Assembly.GetExecutingAssembly()))
                cfg.AddInputStream(stream);

            return cfg;

        }

    }

    /*
     * create table Employee
(
  id int identity not null primary key,
  name varchar(50) not null
)*/

}

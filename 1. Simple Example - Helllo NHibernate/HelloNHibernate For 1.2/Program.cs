using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace HelloNHibernate
{
    public class Program
    {
        static void Main()
        {
            CreateEmployeeAndSaveToDatabase();
            UpdateTobinAndAssignPierreHenriAsManager();
            LoadEmployeesFromDatabase();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void CreateEmployeeAndSaveToDatabase()
        {
            Employee tobin = new Employee();
            tobin.name = "Tobin Harris";
            using (ISession session = OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(tobin);
                    transaction.Commit();
                }
                Console.WriteLine("Saved Tobin to the database");
            }
        }
        static ISession OpenSession()
        {
            if (factory == null)
            {
                Configuration c = new Configuration();
                c.AddAssembly(Assembly.GetCallingAssembly());
                factory = c.BuildSessionFactory();
            }
            return factory.OpenSession();
        }
        static ISessionFactory factory;

        static void LoadEmployeesFromDatabase()
        {
            using (ISession session = OpenSession())
            {
                IQuery query = session.CreateQuery(
                "from Employee as emp order by emp.name asc");
                IList<Employee> foundEmployees = query.List<Employee>();
                Console.WriteLine("\n{0} employees found:",
                foundEmployees.Count);
                foreach (Employee employee in foundEmployees)
                    Console.WriteLine(employee.SayHello());
            }
        }

        static void UpdateTobinAndAssignPierreHenriAsManager()
        {
            using (ISession session = OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery q = session.CreateQuery(
                    "from Employee where name = 'Tobin Harris'");
                    Employee tobin = q.List<Employee>()[0];
                    tobin.name = "Tobin David Harris";
                    Employee pierreHenri = new Employee();
                    pierreHenri.name = "Pierre Henri Kuate";
                    tobin.manager = pierreHenri;
                    transaction.Commit();
                    Console.WriteLine("Updated Tobin and added Pierre Henri");
                }
            }
        }
    }
}
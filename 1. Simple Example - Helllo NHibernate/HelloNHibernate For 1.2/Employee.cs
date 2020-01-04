namespace HelloNHibernate
{
    public class Employee
    {
        public int id;
        public string name;
        public Employee manager;

        public string SayHello()
        {
            return string.Format(
                "'Hello World!', said {0}.", 
                name);
        }
    }
}
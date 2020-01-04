using NHibernateInAction.CaveatEmptor.Model;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="UserDAO"/>.
    /// </summary>
    public class UserDAOImpl : GenericNHibernateDAO<User, long>, UserDAO
    {
    }
}
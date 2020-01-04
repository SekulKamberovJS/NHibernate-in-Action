using NHibernateInAction.CaveatEmptor.Model;

namespace NHibernateInAction.CaveatEmptor.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Model.User"/> entity.
    /// </summary>
    public interface UserDAO : GenericDAO<User, long>
    {
    }
}
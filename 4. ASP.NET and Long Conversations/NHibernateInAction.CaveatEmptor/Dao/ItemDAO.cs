using NHibernateInAction.CaveatEmptor.Model;

namespace NHibernateInAction.CaveatEmptor.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Model.Item"/> entity.
    /// </summary>
    public interface ItemDAO : GenericDAO<Item, long>
    {
        Bid GetMaxBid(long itemId);
        Bid GetMinBid(long itemId);
    }
}
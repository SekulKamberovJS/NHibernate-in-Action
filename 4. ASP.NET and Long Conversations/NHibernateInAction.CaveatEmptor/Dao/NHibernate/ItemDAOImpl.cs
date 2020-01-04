using NHibernate;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Model;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="ItemDAO"/>.
    /// </summary>
    public class ItemDAOImpl : GenericNHibernateDAO<Item, long>, ItemDAO
    {
        #region ItemDAO Members

        public virtual Bid GetMinBid(long itemId)
        {
            try
            {
                // Note the creative where-clause subselect expression..
                IQuery q = Session.GetNamedQuery("MinBid");
                q.SetInt64("itemId", itemId);
                return q.UniqueResult<Bid>();
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }


        public virtual Bid GetMaxBid(long itemId)
        {
            try
            {
                // Note the creative where-clause subselect expression...
                IQuery q = Session.GetNamedQuery("MaxBid");
                q.SetInt64("itemId", itemId);
                return q.UniqueResult<Bid>();
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        #endregion
    }
}
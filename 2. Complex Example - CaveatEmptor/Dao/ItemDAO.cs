////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using NHibernate;
using InfrastructureException = NHibernateInAction.CaveatEmptor.Exceptions.InfrastructureException;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;

namespace NHibernateInAction.CaveatEmptor.Dao
{
    /// <summary>A typical DAO for Auction data access using NHibernate. </summary>
	public class ItemDAO : DAO<Item>
	{
        /// <summary>
        /// Gets the highest Bid for an item.
        /// </summary>
		public virtual Bid GetMaxBid(long itemId)
		{
			Bid maxBidAmount;
			try
			{
                // Query is loaded from an xml file. 
				// Check out the creative where-clause subselect expression...
				IQuery q = NHibernateHelper.Session.GetNamedQuery("maxBid");
				q.SetInt64("itemid", itemId);
				maxBidAmount = (Bid)q.UniqueResult();
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			return maxBidAmount;
		}

        /// <summary>
        /// Gest the lowest Bid for an item.
        /// </summary>
		public virtual Bid GetMinBid(long itemId)
		{
			Bid minBidAmount;
			try
			{
				// Query is loaded from an xml file. 
                // Check out the creative where-clause subselect expression..
				IQuery q = NHibernateHelper.Session.GetNamedQuery("minBid");
				q.SetInt64("itemid", itemId);
				minBidAmount = (Bid) q.UniqueResult();
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			return minBidAmount;
		}
	}
}
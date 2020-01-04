using System.Collections.Generic;
using NHibernate;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of the DAO for Item.
    /// </summary>
    public class ItemDAO_Simple
    {
        public static IList<Item> FindAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria(typeof (Item)).List<Item>();
        }

        public static Item FindById(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Load<Item>(id);
        }

        public static Item MakePersistent(Item entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                session.SaveOrUpdate(entity);
            return entity;
        }

        public static void MakeTransient(Item entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                session.Delete(entity);
        }

        public double GetMaxBidAmount(long itemId)
        {
            string query = @"select max(b.Amount)
	                         from Bid b where b.Item = :item";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery q = session.CreateQuery(query);
                q.SetInt64("itemId", itemId);
                return (double) q.UniqueResult();
            }
        }
    }
}
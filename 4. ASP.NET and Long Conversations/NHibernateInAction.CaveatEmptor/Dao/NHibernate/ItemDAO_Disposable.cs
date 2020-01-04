using System;
using System.Collections.Generic;
using NHibernate;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of the DAO for Item.
    /// </summary>
    public class ItemDAO_Disposable : IDisposable
    {
        private readonly ISession session;

        public ItemDAO_Disposable()
        {
            session = NHibernateHelper.OpenSession();
            session.BeginTransaction();
        }

        #region IDisposable Members

        public void Dispose()
        {
            // This transaction is either an empty one (if we accept previous changes)
            // Or the changes it contains are not supposed to be commited
            session.Transaction.Rollback();
            session.Close();
        }

        #endregion

        public void AcceptChanges()
        {
            // Commiting the transaction closes it and creates a new one
            session.Transaction.Commit();
        }

        public IList<Item> FindAll()
        {
            return session.CreateCriteria(typeof (Item)).List<Item>();
        }

        public Item FindById(long id)
        {
            return session.Load<Item>(id);
        }

        public Item FindByIdAndLock(long id)
        {
            return session.Load<Item>(id, LockMode.Upgrade);
        }

        public Item MakePersistent(Item entity)
        {
            session.SaveOrUpdate(entity);
            return entity;
        }

        public void MakeTransient(Item entity)
        {
            session.Delete(entity);
        }

        public double GetMaxBidAmount(long itemId)
        {
            string query = @"select max(b.Amount)
	                         from Bid b where b.Item = :item";
            IQuery q = session.CreateQuery(query);
            q.SetInt64("itemId", itemId);
            return (double) q.UniqueResult();
        }
    }
}
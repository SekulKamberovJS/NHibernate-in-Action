using System.Collections.Generic;
using NHibernate;
using Example = NHibernate.Expression.Example;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;
using CurrentSessionContext = NHibernate.Context.CurrentSessionContext;

namespace NHibernateInAction.CaveatEmptor.Dao
{
	/// <summary> A typical DAO for auction items using NHibernate. </summary>
	public class SimpleItemDAO : System.IDisposable
	{
		public SimpleItemDAO()
		{
			if( ! CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory) )
			{
				CurrentSessionContext.Bind(NHibernateHelper.SessionFactory.OpenSession());
				NHibernateHelper.GetCurrentSession().BeginTransaction();
			}
		}

		public void AcceptChanges()
		{
			AssertSessionIsBound();
			try
			{
				try
				{
					NHibernateHelper.GetCurrentSession().Transaction.Commit();
				}
				finally
				{
					NHibernateHelper.GetCurrentSession().Close();
				}
			}
			finally
			{
				CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
			}
		}

		public void DiscardChanges()
		{
			AssertSessionIsBound();
			try
			{
				try
				{
					NHibernateHelper.GetCurrentSession().Transaction.Rollback();
				}
				finally
				{
					NHibernateHelper.GetCurrentSession().Close();
				}
			}
			finally
			{
				CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
			}
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		public void Dispose()
		{
			if( ! CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory) )
				return;
			try
			{
				try
				{
					NHibernateHelper.GetCurrentSession().Transaction.Rollback();
				}
				finally
				{
					NHibernateHelper.GetCurrentSession().Close();
				}
			}
			finally
			{
				CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
			}
		}

		private void AssertSessionIsBound()
		{
			if( ! CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory) )
				throw new Exceptions.InfrastructureException("NHibernate session is closed");
		}


		// ********************************************************** //

		public virtual Item GetItemById(long itemId, bool @lock)
		{
			AssertSessionIsBound();
			try
			{
				if(@lock)
				{
					return NHibernateHelper.GetCurrentSession()
									.Load<Item>(itemId, LockMode.Upgrade);
				}
				else
				{
					return NHibernateHelper.GetCurrentSession()
									.Load<Item>(itemId);
				}
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}


		// ********************************************************** //

		public virtual Model.Bid GetMaxBid(long itemId)
		{
			AssertSessionIsBound();
			try
			{
				// Note the creative where-clause subselect expression...
				IQuery q = NHibernateHelper.GetCurrentSession().GetNamedQuery("MaxBid");
				q.SetInt64("itemId", itemId);
				return q.UniqueResult<Model.Bid>();
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}


		public virtual Model.Bid GetMinBid(long itemId)
		{
			AssertSessionIsBound();
			try
			{
				// Note the creative where-clause subselect expression..
				IQuery q = NHibernateHelper.GetCurrentSession().GetNamedQuery("MinBid");
				q.SetInt64("itemId", itemId);
				return q.UniqueResult<Model.Bid>();
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}


		public virtual IList<Item> FindAll()
		{
			AssertSessionIsBound();
			try
			{
				return NHibernateHelper.GetCurrentSession()
					.CreateCriteria(typeof(Item)).List<Item>();
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}


		public virtual IList<Item> FindByExample(Item exampleItem)
		{
			AssertSessionIsBound();
			try
			{
				ICriteria crit = NHibernateHelper.GetCurrentSession()
					.CreateCriteria(typeof(Item));
				return crit.Add(Example.Create(exampleItem))
					.List<Item>();
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}



		public virtual void MakePersistent(Item item)
		{
			AssertSessionIsBound();
			try
			{
				NHibernateHelper.GetCurrentSession().SaveOrUpdate(item);
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}


		public virtual void MakeTransient(Item item)
		{
			AssertSessionIsBound();
			try
			{
				NHibernateHelper.GetCurrentSession().Delete(item);
			}
			catch(HibernateException ex)
			{
				throw new Exceptions.InfrastructureException(ex);
			}
		}
	}
}
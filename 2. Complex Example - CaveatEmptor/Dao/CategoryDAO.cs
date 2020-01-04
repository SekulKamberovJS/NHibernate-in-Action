////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Dao
{
    /// <summary>A typical DAO for Category data access using NHibernate. </summary>
    public class CategoryDAO : DAO<Category>
	{
		public virtual IList<Category> FindAll(bool onlyRootCategories)
		{
			IList<Category> categories;
			try
			{
				if (onlyRootCategories)
				{
					ICriteria crit = NHibernateHelper.Session.CreateCriteria(typeof(Category));
					categories = crit.Add(Expression.IsNull("ParentCategory")).List<Category>();
				}
				else
				{
					categories = NHibernateHelper.Session.CreateCriteria(typeof(Category)).List<Category>();
				}
			}
			catch (HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			return categories;
		}

    }
}
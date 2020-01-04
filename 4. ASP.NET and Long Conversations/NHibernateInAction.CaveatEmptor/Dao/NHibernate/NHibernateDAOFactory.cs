using System;
using NHibernateInAction.CaveatEmptor.Exceptions;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /**
	 * Returns NHibernate-specific instances of DAOs.
	 * <p/>
	 * If for a particular DAO there is no additional non-CRUD functionality, we use
	 * a nested static class to implement the interface in a generic way. This allows clean
	 * refactoring later on, should the interface implement business data access
	 * methods at some later time. Then, we would externalize the implementation into
	 * its own first-level class.
	 */

    public class NHibernateDAOFactory : DAOFactory
    {
        private T GetDAO<T>()
        {
            try
            {
                return (T) Activator.CreateInstance(typeof (T));
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Can not instantiate DAO: " + typeof (T), ex);
            }
        }

        public override UserDAO GetUserDAO()
        {
            // TODO: Why not simply ?
            // return new UserDAOImpl();
            return GetDAO<UserDAOImpl>();
        }

        public override ItemDAO GetItemDAO()
        {
            return GetDAO<ItemDAOImpl>();
        }

        /*
		public CategoryDAO getCategoryDAO() {
			return (CategoryDAO)instantiateDAO(CategoryDAOHibernate.class);
		}

		public CommentDAO getCommentDAO() {
			return (CommentDAO)instantiateDAO(CommentDAOHibernate.class);
		}

		public UserDAO getUserDAO() {
			return (UserDAO)instantiateDAO(UserDAOHibernate.class);
		}

		public CategorizedItemDAO getCategorizedItemDAO() {
			return (CategorizedItemDAO)instantiateDAO(CategorizedItemDAOHibernate.class);
		}

		public BillingDetailsDAO getBillingDetailsDAO() {
			return (BillingDetailsDAO)instantiateDAO(BillingDetailsDAOHibernate.class);
		}

		public ShipmentDAO getShipmentDAO() {
			return (ShipmentDAO)instantiateDAO(ShipmentDAOHibernate.class);
		}*/
    }
}
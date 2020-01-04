namespace NHibernateInAction.CaveatEmptor.Dao
{
    /**
	 * Defines all DAOs and the concrete factories to get the conrecte DAOs.
	 * <p>
	 * To get a concrete DAOFactory, call <tt>Instance()</tt> with one of the
	 * classes that extend this factory.
	 * <p>
	 * Implementation: If you write a new DAO, this class has to know about it.
	 * If you add a new persistence mechanism, add an additional concrete factory
	 * for it as a constant, like <tt>NHibernate</tt>.
	 */

    public abstract class DAOFactory
    {
        /**
		 * Creates a standalone DAOFactory that returns unmanaged DAO
		 * beans for use in any environment Hibernate has been configured
		 * for. Uses HibernateUtil/SessionFactory and Hibernate context
		 * propagation (CurrentSessionContext), thread-bound or transaction-bound,
		 * and transaction scoped.
		 */
// TODO: Remove?		public static readonly System.Type NHibernate = typeof(NHibernate.NHibernateDAOFactory);

        /**
		 * Factory method for instantiation of concrete factories.
		 */
/* TODO: Remove?		public static DAOFactory Instance(System.Type factory) {
			try {
				return (DAOFactory)System.Activator.CreateInstance(factory);
			} catch (System.Exception ex) {
				throw new Exceptions.InfrastructureException("Couldn't create DAOFactory: " + factory, ex);
			}
		}*/

        // Add your DAO interfaces here
        public abstract UserDAO GetUserDAO();
        public abstract ItemDAO GetItemDAO();
        /*public abstract CategoryDAO getCategoryDAO();
		public abstract CommentDAO getCommentDAO();
		public abstract CategorizedItemDAO getCategorizedItemDAO();
		public abstract BillingDetailsDAO getBillingDetailsDAO();
		public abstract ShipmentDAO getShipmentDAO();*/
    }
}
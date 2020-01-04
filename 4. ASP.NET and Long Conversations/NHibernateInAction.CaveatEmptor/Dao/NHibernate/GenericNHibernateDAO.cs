using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Dao.NHibernate
{
    /**
	 * Implements the generic CRUD data access operations using Hibernate APIs.
	 * <p>
	 * To write a DAO, subclass and parameterize this class with your persistent class.
	 * Of course, assuming that you have a traditional 1:1 appraoch for Entity:DAO design.
	 * <p>
	 * You have to inject a current Hibernate <tt>Session</tt> to use a DAO. Otherwise, this
	 * generic implementation will use <tt>HibernateUtil.getSessionFactory()</tt> to obtain the
	 * curren <tt>Session</tt>.
	 *
	 * @see NHibernateDAOFactory
	 */

    public abstract class GenericNHibernateDAO<T, ID> : GenericDAO<T, ID>
    {
        private ISession session;

        public ISession Session
        {
            get
            {
                if (session == null)
                    session = NHibernateHelper.GetCurrentSession();
                return session;
            }
            set { session = value; }
        }

        #region GenericDAO<T,ID> Members

        public T FindById(ID id)
        {
            try
            {
                return Session.Load<T>(id);
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        public T FindByIdAndLock(ID id)
        {
            try
            {
                return Session.Load<T>(id, LockMode.Upgrade);
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        public IList<T> FindAll()
        {
            return FindByCriteria();
        }

        public IList<T> FindByExample(T exampleInstance, params string[] excludeProperty)
        {
            ICriteria crit = Session.CreateCriteria(typeof (T));
            Example example = Example.Create(exampleInstance);
            foreach (string exclude in excludeProperty)
                example.ExcludeProperty(exclude);
            crit.Add(example);
            return crit.List<T>();
        }

        public T MakePersistent(T entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public void MakeTransient(T entity)
        {
            Session.Delete(entity);
        }

        public void Flush()
        {
            Session.Flush();
        }

        public void Clear()
        {
            Session.Clear();
        }

        #endregion

        /// <summary>
        /// Use this inside subclasses as a convenience method.
        /// </summary>
        protected IList<T> FindByCriteria(params ICriterion[] criterion)
        {
            ICriteria crit = Session.CreateCriteria(typeof (T));
            foreach (ICriterion c in criterion)
                crit.Add(c);
            return crit.List<T>();
        }
    }
}
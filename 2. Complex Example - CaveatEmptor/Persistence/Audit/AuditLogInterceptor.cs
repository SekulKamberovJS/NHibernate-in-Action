using NHibernate;
using NHibernate.Type;
using Iesi.Collections;
using NHibernateInAction.CaveatEmptor.Model;


namespace NHibernateInAction.CaveatEmptor.Persistence.Audit
{
	public class AuditLogInterceptor : EmptyInterceptor
	{
		public virtual ISession Session
		{
			set { session = value; }
		}

		public virtual long UserId
		{
			set { userId = value; }
		}

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private ISession session;
		private long userId;

		private readonly ISet inserts = new HashedSet();
		private readonly ISet updates = new HashedSet();


		public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			if(entity is IAuditable)
				inserts.Add(entity);

			else if (entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), false).Length > 0)
				inserts.Add(entity);

			return false;
		}


		public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
		{
			if (!inserts.Contains(entity))
			{
				if (entity is IAuditable)
					updates.Add(entity);

				else if (entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), false).Length > 0)
					updates.Add(entity);
			}

			return false;
		}


		public override void PostFlush(System.Collections.ICollection c)
		{
			try
			{
				foreach(object entity in inserts)
				{
					log.Debug("Intercepted creation of : " + entity);
					AuditLog.LogEvent(LogType.Create, entity, userId, session.Connection);
				}
				foreach(object entity in updates)
				{
					log.Debug("Intercepted modification of : " + entity);
					AuditLog.LogEvent(LogType.Update, entity, userId, session.Connection);
				}
			}
			catch(HibernateException ex)
			{
				throw new CallbackException(ex);
			}
			finally
			{
				inserts.Clear();
				updates.Clear();
			}
		}
	}
}
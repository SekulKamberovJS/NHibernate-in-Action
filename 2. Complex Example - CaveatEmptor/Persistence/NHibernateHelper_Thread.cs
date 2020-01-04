using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernateInAction.CaveatEmptor.Exceptions;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> Basic NHibernate helper class, handles ISessionFactory, ISession and ITransaction.
	/// 
	/// Uses a static constructor for the initial ISessionFactory creation
	/// and holds ISession and ITransactions in thread local variables.
	/// All exceptions are wrapped in an InfrastructureException.
	/// </summary>
	public class NHibernateHelper_Thread
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static Configuration configuration;
		private static ISessionFactory sessionFactory;
		private static readonly System.LocalDataStoreSlot threadSession = System.Threading.Thread.AllocateDataSlot();
		private static readonly System.LocalDataStoreSlot threadTransaction = System.Threading.Thread.AllocateDataSlot();
		private static readonly System.LocalDataStoreSlot threadInterceptor = System.Threading.Thread.AllocateDataSlot();


		/// <summary> Returns the SessionFactory used for this static class. </summary>
		public static ISessionFactory SessionFactory
		{
			get
			{
				return sessionFactory;
			}
		}

		/// <summary> Returns the original NHibernate configuration. </summary>
		public static Configuration Configuration
		{
			get { return configuration; }
		}

		/// <summary> Retrieves the current Session local to the thread.
		/// If no Session is open, opens a new Session for the running thread.
		/// </summary>
		public static ISession Session
		{
			get
			{
				ISession s = (ISession) System.Threading.Thread.GetData(threadSession);
				try
				{
					if(s == null)
					{
						log.Debug("Opening new Session for this thread.");
						if(Interceptor != null)
						{
							log.Debug("Using interceptor: " + Interceptor.GetType());
							s = SessionFactory.OpenSession(Interceptor);
						}
						else
						{
							s = SessionFactory.OpenSession();
						}
						System.Threading.Thread.SetData(threadSession, s);
					}
				}
				catch(HibernateException ex)
				{
					throw new InfrastructureException(ex);
				}
				return s;
			}
		}

		private static IInterceptor Interceptor
		{
			get
			{
				IInterceptor interceptor = (IInterceptor) System.Threading.Thread.GetData(threadInterceptor);
				return interceptor;
			}
		}


		/// <summary> Rebuild the SessionFactory with the static Configuration. </summary>
		public static void RebuildSessionFactory()
		{
			lock(sessionFactory)
			{
				try
				{
					sessionFactory = Configuration.BuildSessionFactory();
				}
				catch(System.Exception ex)
				{
					throw new InfrastructureException(ex);
				}
			}
		}


		/// <summary> Rebuild the SessionFactory with the given NHibernate Configuration. </summary>
		public static void RebuildSessionFactory(Configuration cfg)
		{
			lock(sessionFactory)
			{
				try
				{
					sessionFactory = cfg.BuildSessionFactory();
					configuration = cfg;
				}
				catch(System.Exception ex)
				{
					throw new InfrastructureException(ex);
				}
			}
		}


		/// <summary> Closes the Session local to the thread.</summary>
		public static void CloseSession()
		{
			try
			{
				ISession s = (ISession) System.Threading.Thread.GetData(threadSession);
				System.Threading.Thread.SetData(threadSession, null);
				if(s != null && s.IsOpen)
				{
					log.Debug("Closing Session of this thread.");
					s.Close();
				}
				else
					log.Warn("Useless call of CloseSession().");
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
		}


		/// <summary> Start a new database transaction.</summary>
		public static void BeginTransaction()
		{
			ITransaction tx = (ITransaction) System.Threading.Thread.GetData(threadTransaction);
			try
			{
				if(tx == null)
				{
					log.Debug("Starting new database transaction in this thread.");
					tx = Session.BeginTransaction();
					System.Threading.Thread.SetData(threadTransaction, tx);
				}
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
		}


		/// <summary> Commit the database transaction.</summary>
		public static void CommitTransaction()
		{
			ITransaction tx = (ITransaction) System.Threading.Thread.GetData(threadTransaction);
			try
			{
				if(tx != null && !tx.WasCommitted && !tx.WasRolledBack)
				{
					log.Debug("Committing database transaction of this thread.");
					tx.Commit();
				}
				System.Threading.Thread.SetData(threadTransaction, null);
			}
			catch(HibernateException ex)
			{
				RollbackTransaction();
				throw new InfrastructureException(ex);
			}
		}


		/// <summary> Commit the database transaction.</summary>
		public static void RollbackTransaction()
		{
			ITransaction tx = (ITransaction) System.Threading.Thread.GetData(threadTransaction);
			try
			{
				System.Threading.Thread.SetData(threadTransaction, null);
				if(tx != null && !tx.WasCommitted && !tx.WasRolledBack)
				{
					log.Debug("Tyring to rollback database transaction of this thread.");
					tx.Rollback();
				}
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			finally
			{
				CloseSession();
			}
		}


		/// <summary> Reconnects a NHibernate Session to the current Thread. </summary>
		/// <param name="session">The NHibernate Session to be reconnected. </param>
		public static void Reconnect(ISession session)
		{
			try
			{
				session.Reconnect();
				System.Threading.Thread.SetData(threadSession, session);
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
		}


		/// <summary> Disconnect and return Session from current Thread. </summary>
		/// <returns> the disconnected Session </returns>
		public static ISession DisconnectSession()
		{
			ISession session = Session;
			try
			{
				System.Threading.Thread.SetData(threadSession, null);
				if(session.IsConnected && session.IsOpen)
					session.Disconnect();
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			return session;
		}


		/// <summary> Register a NHibernate interceptor with the current thread.
		/// 
		/// Every Session opened is opened with this interceptor after
		/// registration. Has no effect if the current Session of the
		/// thread is already open, effective on next close()/getSession().
		/// </summary>
		public static void RegisterInterceptor(IInterceptor interceptor)
		{
			System.Threading.Thread.SetData(threadInterceptor, interceptor);
		}


		static NHibernateHelper_Thread()
		{
			// Create the initial SessionFactory from the default configuration files
			try
			{
				configuration = new Configuration();
				sessionFactory = configuration.Configure().BuildSessionFactory();
			}
			catch(Exception ex)
			{
				log.Error("Building SessionFactory failed.", ex);
				throw new System.Exception("Building SessionFactory failed", ex);
			}
		}
	}
}
using System;
using System.Data;
using System.EnterpriseServices;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using NHibernate.Cfg;
using NHibernateInAction.CaveatEmptor.Exceptions;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> COM+ enabled NHibernate helper class, handles ISessionFactory, ISession and ITransaction.
	/// 
	/// Uses a static constructor for the initial ISessionFactory creation
	/// and holds ISession and ITransactions in Remoting's CallContext.
	/// All exceptions are wrapped in an InfrastructureException.
	/// </summary>
	[Transaction(TransactionOption.Supported)]
	public class NHibernateHelper_EntSrvs : ServicedComponent
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static Configuration configuration;
		private static ISessionFactory sessionFactory;
		private static readonly string uniqueName = Guid.NewGuid().ToString();
		private static readonly string nameSession = uniqueName + "_NHibernate.ISession";
		private static readonly string nameInterceptor = uniqueName + "_NHibernate.IInterceptor";


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

		/// <summary> Retrieves the current Session local to the context.
		/// If no Session is open, opens a new Session for the current context.
		/// </summary>
		public static ISession Session
		{
			get
			{
				ISession s = (ISession) CallContext.GetData(nameSession);
				try
				{
					if(s == null)
					{
						log.Debug("Opening new Session for this context.");
						if(Interceptor != null)
						{
							log.Debug("Using interceptor: " + Interceptor.GetType());
							s = SessionFactory.OpenSession(Interceptor);
						}
						else
						{
							s = SessionFactory.OpenSession();
						}
						CallContext.SetData(nameSession, s);
						TryEnlistDistributedTransaction();
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
				IInterceptor interceptor = (IInterceptor) CallContext.GetData(nameInterceptor);
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


		private static void TryEnlistDistributedTransaction()
		{
			if (ContextUtil.IsInTransaction)
			{
				IDbConnection conn = Session.Connection;
				MethodInfo mi = conn.GetType().GetMethod("EnlistDistributedTransaction", BindingFlags.Public | BindingFlags.Instance);
				if (mi != null)
					mi.Invoke(conn, new object[] { (System.EnterpriseServices.ITransaction)ContextUtil.Transaction });
			}
		}


		/// <summary> Closes the Session local to the context.</summary>
		public static void CloseSession()
		{
			try
			{
				ISession s = (ISession) CallContext.GetData(nameSession);
				CallContext.SetData(nameSession, null);
				if(s != null && s.IsOpen)
				{
					log.Debug("Closing Session of this context.");
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
			log.Debug("Starting new COM+ transaction.");
			ServiceConfig sc = new ServiceConfig();
			sc.Transaction = TransactionOption.RequiresNew;
			ServiceDomain.Enter(sc);
		}


		/// <summary> Commit the database transaction.</summary>
		public static void CommitTransaction()
		{
			try
			{
				log.Debug("Committing database transaction of this context.");
				ContextUtil.SetComplete();
				ServiceDomain.Leave();
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
			try
			{
				log.Debug("Trying to rollback database transaction of this context.");
				ContextUtil.SetAbort();
				ServiceDomain.Leave();
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


		/// <summary> Reconnects a NHibernate Session to the current context. </summary>
		/// <param name="session">The NHibernate Session to be reconnected. </param>
		public static void Reconnect(ISession session)
		{
			try
			{
				session.Reconnect();
				CallContext.SetData(nameSession, session);
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
		}


		/// <summary> Disconnect and return Session from current context. </summary>
		/// <returns> the disconnected Session </returns>
		public static ISession DisconnectSession()
		{
			ISession session = Session;
			try
			{
				CallContext.SetData(nameSession, null);
				if(session.IsConnected && session.IsOpen)
					session.Disconnect();
			}
			catch(HibernateException ex)
			{
				throw new InfrastructureException(ex);
			}
			return session;
		}


		/// <summary> Register a NHibernate interceptor with the current context.
		/// 
		/// Every Session opened after this registration is opened with
		/// this interceptor. Has no effect if the current Session of the
		/// context is already open, effective on next Close()/Session.
		/// </summary>
		public static void RegisterInterceptor(IInterceptor interceptor)
		{
			CallContext.SetData(nameInterceptor, interceptor);
		}


		static NHibernateHelper_EntSrvs()
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
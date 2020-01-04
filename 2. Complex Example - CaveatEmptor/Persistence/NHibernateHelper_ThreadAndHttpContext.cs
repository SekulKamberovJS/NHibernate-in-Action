using System;
using System.Collections;
using System.Threading;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

// TODO: Create an "ISessionStorage" interface with properties Session and Transaction (get and set)
// Then implement it for ThreadContext, HttpContext, CallContext, etc.
// Make NHibernateHelper select a SessionStorage and use it...
// Cf. Sample - Wiki_AndrewMayorovAspNet.zip

/* In Web.config
<httpModules>
	<add name="NHibernateHelper_ThreadAndHttpContext"
		type="NHibernateInAction.CaveatEmptor.Persistence.HttpRequestModule, NHibernateInAction.CaveatEmptor" />
</httpModules>
*/

/* HttpRequestModule.cs
public class HttpRequestModule : IHttpModule
{
	public String ModuleName
	{
		get { return "NHibernateHelper_ThreadAndHttpContext"; }
	}
 
	public void Init(HttpApplication application)
	{
		application.BeginRequest += new EventHandler(this.Application_BeginRequest);
		application.EndRequest += new EventHandler(this.Application_EndRequest);
		application.Error += new EventHandler(this.Application_Error);
	}

	private void Application_BeginRequest(object source, EventArgs e)
	{
		// Note: This is useless (because the session will be eventually created when needed)
		// And it may be a waste of ressources because it might not be used at all
		NHibernateHelper_ThreadAndHttpContext.GetSession();
	}

	private void Application_EndRequest(object source, EventArgs e)
	{
		NHibernateHelper_ThreadAndHttpContext.CloseSession();
	}

	private void Application_Error(object sender, EventArgs e)
	{
		// TODO: Useful to add?
		// Most of the time, it should be possible to catch the error and rollback
		NHibernateHelper_ThreadAndHttpContext.RollbackAndCloseSession();
	}

	public void Dispose() {}
}*/

namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary>
	/// Provides "Thread Context" and HttpContext to manage sessions.
	/// </summary>
	public class NHibernateHelper_ThreadAndHttpContext
	{
		#region private static variables

		private static readonly ISessionFactory _sessionFactory;
		private static Hashtable _threadSessions = new Hashtable();  
		private static readonly bool isHttpContextAvailable = (HttpContext.Current != null);
		private const string _httpContextKey = "NHibernate.ISession";

		#endregion
 

		#region constructors
 
		private NHibernateHelper_ThreadAndHttpContext() {}
 
		static NHibernateHelper_ThreadAndHttpContext()
		{
			try 
			{  
				Configuration cfg = new Configuration();
				_sessionFactory = cfg.Configure().BuildSessionFactory();  
			} 
			catch (Exception ex) 
			{
				throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().Name + " error: ", ex);
			}
		}
 
		#endregion
 

		# region private static methods
 
		/// <summary>
		/// gets the current session   
		/// </summary>
		private static ISession GetCurrentSession()
		{
			ISession session = null;
			if (!isHttpContextAvailable)
				session = GetCurrentThreadSession();
			else
				session = GetCurrentHttpContextSession();
			return session;
		}
 
		/// <summary>
		/// sets the current session   
		/// </summary>
		private static void StoreCurrentSession(ISession session)
		{
			if (!isHttpContextAvailable)
				StoreCurrentThreadSession(session);
			else
				StoreCurrentHttpContextSession(session);
		}
 
		/// <summary>
		/// sets the current session   
		/// </summary>
		private static void RemoveCurrentSession()
		{
			if (!isHttpContextAvailable)
				RemoveCurrentThreadSession();
			else
				RemoveCurrentHttpContextSession();
		}
 
		# endregion
 

		#region private static methods - HttpContext related
 
		/// <summary>
		/// gets the session for the current thread
		/// </summary>
		private static ISession GetCurrentHttpContextSession()
		{
			if (HttpContext.Current.Items.Contains(_httpContextKey))
				return (ISession)HttpContext.Current.Items[_httpContextKey];
			return null;
		}
 
		private static void StoreCurrentHttpContextSession(ISession session)
		{
			if (HttpContext.Current.Items.Contains(_httpContextKey))
				HttpContext.Current.Items[_httpContextKey] = session;
			else 
				HttpContext.Current.Items.Add(_httpContextKey, session);
		}
 
		/// <summary>
		/// remove the session for the currennt HttpContext
		/// </summary>
		private static void RemoveCurrentHttpContextSession()
		{  
			if (HttpContext.Current.Items.Contains(_httpContextKey))
			{HttpContext.Current.Items.Remove(_httpContextKey);}
		}
 
		#endregion
 

		#region private static methods - ThreadContext related
 
		/// <summary>
		/// gets the session for the current thread
		/// </summary>
		private static ISession GetCurrentThreadSession()
		{
			ISession session = null;
			Thread threadCurrent = Thread.CurrentThread;
			if (threadCurrent.Name == null)
				threadCurrent.Name = Guid.NewGuid().ToString();
			else 
			{
				object threadSession = _threadSessions[threadCurrent.Name];
				if (threadSession != null)
					session = (ISession)threadSession;
			}
			return session;
		}
 
		private static void StoreCurrentThreadSession(ISession session)
		{
			if (_threadSessions.Contains(Thread.CurrentThread.Name))
				_threadSessions[Thread.CurrentThread.Name] = session;
			else
				_threadSessions.Add(Thread.CurrentThread.Name, session);
		}
 
		private static void RemoveCurrentThreadSession()
		{
			if (_threadSessions.Contains(Thread.CurrentThread.Name))
				_threadSessions.Remove(Thread.CurrentThread.Name);
		}
 
		#endregion
 

		#region public static methods
 
		/// <summary>
		/// gets a session (creates new one if none available) 
		/// </summary>
		/// <returns>a session</returns>
		public static ISession GetSession()
		{
			ISession session = GetCurrentSession();
			if (session == null) 
				session = _sessionFactory.OpenSession();
			if (!session.IsConnected)
				session.Reconnect(); 
			StoreCurrentSession(session);
			return session;
		}
 
		/// <summary>
		/// closes the current session
		/// </summary>
		public static void CloseSession()
		{
			ISession session = GetCurrentSession();
			if (session == null)
				return; 
			if (!session.IsConnected)
				session.Reconnect(); 
			session.Flush();
			session.Close();
			RemoveCurrentSession();
		}
 
		/// <summary>
		/// disconnects the current session (if not in active transaction)
		/// </summary>
		public static void DisconnectSession()
		{
			ISession session = GetCurrentSession();
			if (session == null) return; 
			if (!session.IsConnected) return;
			if (session.Transaction == null) 
				session.Disconnect();
			else
			{
				if (session.Transaction.WasCommitted || session.Transaction.WasRolledBack)
					session.Disconnect();  
			}
		}
 
		#endregion
	}
}

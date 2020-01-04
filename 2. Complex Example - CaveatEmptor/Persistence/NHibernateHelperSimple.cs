using System;
using NHibernate;
using Configuration = NHibernate.Cfg.Configuration;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> A very simple NHibernate helper class that holds the SessionFactory as a singleton.
	/// 
	/// The only job of this helper class is to give your application code easy
	/// access to the SessionFactory. It initializes the SessionFactory
	/// when it is loaded (static constructor) and you can easily open new
	/// sessions. Only really useful for trivial applications.
	/// </summary>
	public class NHibernateHelperSimple
	{
		public static readonly ISessionFactory SessionFactory;

		static NHibernateHelperSimple()
		{
			// Create the initial SessionFactory from the default configuration files
			try
			{
				Configuration cfg = new Configuration();
				SessionFactory = cfg.Configure().BuildSessionFactory();
			}
			catch(Exception ex)
			{
				throw new Exception("Building SessionFactory failed", ex);
			}
		}

		public static ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}
	}
}
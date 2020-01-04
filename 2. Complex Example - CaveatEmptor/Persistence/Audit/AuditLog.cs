using System.Reflection;
using NHibernate;


namespace NHibernateInAction.CaveatEmptor.Persistence.Audit
{
	/// <summary> The audit log helper that logs actual events.
	/// 
	/// The logEvent() method needs an ADO.NET connection, it will
	/// open a new NHibernate Session on that connection and
	/// persist the event. The temporary Session is then closed,
	/// transaction handling is left to the client calling this method.
	/// </summary>
	public class AuditLog
	{
		public static void LogEvent(LogType logType, object entity, long userId, System.Data.IDbConnection connection)
		{
            //start new session because the underlying session can be in a fragile statue during the 
            //interceptor calls
			using( ISession tempSession = NHibernateHelper.SessionFactory.OpenSession() )
			{
			    long entityId = -1;
			    PropertyInfo idProperty = entity.GetType().GetProperty("Id",BindingFlags.GetProperty|BindingFlags.Public|BindingFlags.Instance);
                if(idProperty != null)
                {
                    entityId = (long) idProperty.GetValue(entity,null); 
                }
                
				AuditLogRecord record = new AuditLogRecord(logType.ToString(), entityId, entity.GetType(), userId);

                //updates will be written to the EXISTING connection
                //which will mean that calling code manages transactions and also
                //it's quick (rather than opening new session)
				tempSession.Save(record);
				tempSession.Flush();
			}
		}
	}
}
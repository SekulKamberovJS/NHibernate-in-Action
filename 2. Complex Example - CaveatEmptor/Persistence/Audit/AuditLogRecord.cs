using System;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Model;

/*
This mapping uses an internally managed identifier and access
fields of the AuditLogRecord class directly. Keep it simple,
this part of our persistence layer doesn't need more abstraction
and encapsulation.
*/

namespace NHibernateInAction.CaveatEmptor.Persistence.Audit
{
	/// <summary> A trivial audit log record.
	/// This simple value class represents a single audit event.
	/// </summary>
	[Class(Table="AUDIT_LOG_RECORD", Lazy = false)]
	public class AuditLogRecord
	{
        [Id(0, Name = "Id", Type = "long", Access = "field")]
        [Generator(1, Class = "native")]
		public long Id;

        [Property(Access = "field")]
		public string Message;

        [Property(Access = "field")]
		public long EntityId;

        [Property(Access = "field")]
		public Type EntityType;

        [Property(Access = "field")]
		public long UserId;

        [Property(Access = "field")]
		public DateTime Created;

        [Property(Access = "field")]
	    public Type EntityClass;


		internal AuditLogRecord() // For NHibernate
		{
		}


		public AuditLogRecord(string message, long entityId, System.Type entityType, long userId)
		{
			Message = message;
			EntityId = entityId;
			EntityType = entityType;
			UserId = userId;
            Created = SystemTime.NowWithoutMilliseconds;
		}
	}
}
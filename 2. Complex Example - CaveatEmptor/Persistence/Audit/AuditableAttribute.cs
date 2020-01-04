using System;

namespace NHibernateInAction.CaveatEmptor.Persistence.Audit
{
	/// <summary>
	/// Summary description for AuditableAttribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple=false )]
	[Serializable]
	public class AuditableAttribute : Attribute
	{
	}
}

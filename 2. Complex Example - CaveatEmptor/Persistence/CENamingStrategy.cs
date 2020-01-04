using System;
using NHibernate.Cfg;
using NHibernate.Util;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> Prefix database table and column names with a CaveatEmptor handle.
	/// 
	/// This is the implementation of a NHibernate NamingStrategy.
	/// NHibernate calls this class whenever it creates the database schema.
	/// All table names are prefixed with "CE_" while keeping the
	/// default NHibernate of uppercase property names. To enable this strategy,
	/// set it as the default for the SessionFactory , eg. in
	/// HibernateUtil:
	/// 
	/// configuration = new Configuration();
	/// configuration.setNamingStrategy(new CENamingStrategy());
	/// sessionFactory = configuration.configure().buildSessionFactory();
	/// 
	/// In general, NamingStrategy is a powerful concept that gives
	/// you freedom to name your database tables and columns using whatever
	/// pattern you like.
	/// </summary>
	/// <seealso cref="HibernateUtil">
	/// </seealso>
	public class CENamingStrategy : INamingStrategy
	{
		public virtual string ClassToTableName(string className)
		{
			return StringHelper.Unqualify(className);
		}


		public virtual string PropertyToColumnName(string propertyName)
		{
			return propertyName;
		}


		public virtual string TableName(string tableName)
		{
			return "CE_" + tableName;
		}


		public virtual string ColumnName(string columnName)
		{
			return columnName;
		}


		public virtual string PropertyToTableName(string className, string propertyName)
		{
			return "CE_" + ClassToTableName(className) + '_' + PropertyToColumnName(propertyName);
		}
	}
}
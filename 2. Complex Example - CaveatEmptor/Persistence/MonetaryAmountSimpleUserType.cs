using System;
using System.Data;
using NHibernate;
using MonetaryAmount = NHibernateInAction.CaveatEmptor.Model.MonetaryAmount;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> This is a simple NHibernate custom mapping type for MonetaryAmount value types.
	/// 
	/// Note that this mapping type is for legacy databases that only have a
	/// single numeric column to hold monetary amounts. Every MonetaryAmount
	/// will be converted to USD (using some conversion magic of the class itself)
	/// and saved to the database. Every value read from the database will be
	/// converted to the currency prefered by the current user (UserSession thread local).
	/// </summary>
	public class MonetaryAmountSimpleUserType : NHibernate.UserTypes.IUserType
	{
		private static readonly NHibernate.SqlTypes.SqlType[] SQL_TYPES =
			{NHibernateUtil.Double.SqlType};

		public NHibernate.SqlTypes.SqlType[] SqlTypes
		{
			get { return SQL_TYPES; }
		}

		public Type ReturnedType
		{
			get { return typeof(MonetaryAmount); }
		}


		new public bool Equals(object x, object y)
		{
			if(ReferenceEquals(x, y))
				return true;
			if(x == null || y == null)
				return false;
			return x.Equals(y);
		}

		///<summary> Get a hashcode for the instance, consistent with persistence "equality" </summary>
		public int GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}


		public object DeepCopy(object value)
		{
			return value; // MonetaryAmount is immutable
		}

		///<summary>
		/// During merge, replace the existing (<paramref name="target" />) value in the entity
		/// we are merging to with a new (<paramref name="original" />) value from the detached
		/// entity we are merging. For immutable objects, or null values, it is safe to simply
		/// return the first parameter. For mutable objects, it is safe to return a copy of the
		/// first parameter. For objects with component values, it might make sense to
		/// recursively replace component values.
		///</summary>
		///<param name="original">the value from the detached entity being merged</param>
		///<param name="target">the value in the managed entity</param>
		///<param name="owner">the managed entity</param>
		///<returns>
		///the value to be merged
		///</returns>
		public object Replace(object original, object target, object owner)
		{
			return original; // MonetaryAmount is immutable
		}

		///<summary>
		/// Reconstruct an object from the cacheable representation. At the very least this
		/// method should perform a deep copy if the type is mutable. (optional operation)
		///</summary>
		///<param name="cached">the object to be cached</param>
		///<param name="owner">the owner of the cached object</param>
		///<returns>
		///a reconstructed object from the cachable representation
		///</returns>
		public object Assemble(object cached, object owner)
		{
			return cached; // MonetaryAmount is immutable
		}

		///<summary>
		/// Transform the object into its cacheable representation. At the very least this
		/// method should perform a deep copy if the type is mutable. That may not be enough
		/// for some implementations, however; for example, associations must be cached as
		/// identifier values. (optional operation)
		///</summary>
		///<param name="value">the object to be cached</param>
		///<returns>
		///a cacheable representation of the object
		///</returns>
		public object Disassemble(object value)
		{
			return value; // MonetaryAmount is immutable
		}


		public bool IsMutable
		{
			get { return false; }
		}


		public object NullSafeGet(IDataReader dr, string[] names, object owner)
		{
			object obj = NHibernateUtil.Double.NullSafeGet(dr, names[0]);
			if(obj == null) return null;
			double valueInUSD = (double) obj;
			return new MonetaryAmount(valueInUSD, "USD");
		}


		public void NullSafeSet(IDbCommand cmd, object obj, int index)
		{
			if(obj == null)
			{
				((IDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
			}
			else
			{
				MonetaryAmount anyCurrency = (MonetaryAmount) obj;
				MonetaryAmount amountInUSD =
					MonetaryAmount.Convert(anyCurrency, "USD");
				((IDataParameter) cmd.Parameters[index]).Value = amountInUSD.Value;
			}
		}
	}
}
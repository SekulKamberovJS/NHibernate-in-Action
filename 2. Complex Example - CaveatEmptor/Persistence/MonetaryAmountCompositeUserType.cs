using System;
using System.Data;
using NHibernate;
using MonetaryAmount = NHibernateInAction.CaveatEmptor.Model.MonetaryAmount;


namespace NHibernateInAction.CaveatEmptor.Persistence
{
	/// <summary> This is a simple NHibernate custom mapping type for MonetaryAmount value types.
	/// 
	/// Basically the same as the simple MonetaryAmountSimpleUserType, but
	/// implementing the NHibernate CompositeUserType interface. This interface
	/// has some additional methods that allow NHibernate to analyze the value type you
	/// are mapping. This is mostly useful for HQL queries: with this custom mapping
	/// type, you can use the "amount" and "currency" sub-components in HQL queries.
	/// 
	/// </summary>
	/// <seealso cref="MonetaryAmountSimpleUserType">
	/// </seealso>
	public class MonetaryAmountCompositeUserType : NHibernate.UserTypes.ICompositeUserType
	{
		public Type ReturnedClass
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
		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}


		public object DeepCopy(object value)
		{
			return value; // MonetaryAmount is immutable
		}


		public bool IsMutable
		{
			get { return false; }
		}


		public object NullSafeGet(IDataReader dr, string[] names,
		                          NHibernate.Engine.ISessionImplementor session, object owner)
		{
			object obj0 = NHibernateUtil.Double.NullSafeGet(dr, names[0]);
			object obj1 = NHibernateUtil.String.NullSafeGet(dr, names[1]);
			if(obj0 == null || obj1 == null) return null;
			double value = (double) obj0;
			string currency = (string) obj1;
			return new MonetaryAmount(value, currency);
		}


		public void NullSafeSet(IDbCommand cmd, object obj, int index,
		                        NHibernate.Engine.ISessionImplementor session)
		{
			if(obj == null)
			{
				((IDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
				((IDataParameter) cmd.Parameters[index + 1]).Value = DBNull.Value;
			}
			else
			{
				MonetaryAmount amount = (MonetaryAmount) obj;
				((IDataParameter) cmd.Parameters[index]).Value = amount.Value;
				((IDataParameter) cmd.Parameters[index + 1]).Value = amount.Currency;
			}
		}


		public string[] PropertyNames
		{
			get { return new string[] {"Value", "Currency"}; }
		}

		public NHibernate.Type.IType[] PropertyTypes
		{
			get
			{
				return new NHibernate.Type.IType[]
					{
						NHibernateUtil.Double, NHibernateUtil.String
					};
			}
		}


		public object GetPropertyValue(object component, int property)
		{
			MonetaryAmount amount = (MonetaryAmount) component;
			if(property == 0)
				return amount.Value;
			else
				return amount.Currency;
		}


		public void SetPropertyValue(object comp, int property, object value)
		{
			throw new Exception("Immutable!");
		}


		public object Assemble(object cached,
		                       NHibernate.Engine.ISessionImplementor session, object owner)
		{
			return cached;
		}

		///<summary>
		/// During merge, replace the existing (target) value in the entity we are merging to
		/// with a new (original) value from the detached entity we are merging. For immutable
		/// objects, or null values, it is safe to simply return the first parameter. For
		/// mutable objects, it is safe to return a copy of the first parameter. However, since
		/// composite user types often define component values, it might make sense to recursively 
		/// replace component values in the target object.
		///</summary>
		public object Replace(object original, object target, NHibernate.Engine.ISessionImplementor session, object owner)
		{
			return original; // MonetaryAmount is immutable
		}


		public object Disassemble(object value,
		                          NHibernate.Engine.ISessionImplementor session)
		{
			return value;
		}
	}
}
////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;

namespace NHibernateInAction.CaveatEmptor.Model
{
	/// <summary> Represents a monetary amount as value and currency. </summary>
	[Serializable]
	public class MonetaryAmount
	{
		public virtual string Currency
		{
			get { return _currency; }

		}

		public virtual double Value
		{
			get { return _value; }

		}

		private double _value;
		private string _currency;


		public MonetaryAmount(double value, string currency)
		{
			this._value = value;
			this._currency = currency;
		}


	    #region Common Methods

	    public override bool Equals(object o)
	    {
	        if(this == o)
	            return true;

	        if(!(o is MonetaryAmount))
	            return false;

	        MonetaryAmount monetaryAmount = (MonetaryAmount) o;

	        if(Currency != monetaryAmount.Currency)
	            return false;

	        if(Value != monetaryAmount.Value)
	            return false;

	        return true;
	    }

	    public override int GetHashCode()
	    {
	        int result;
	        result = Value.GetHashCode();
	        result = 29*result + Currency.GetHashCode();
	        return result;
	    }


	    public override string ToString()
	    {
	        return "Value: '" + Value + "', " + "Currency: '" + Currency + "'";
	    }


	    public virtual int CompareTo(object o)
	    {
	        if(o is MonetaryAmount)
	        {
	            // TODO: This would actually require some currency conversion magic
	            return this.Value.CompareTo(((MonetaryAmount) o).Value);
	        }
	        return 0;
	    }

	    #endregion

	    #region Business Methods

	    public static MonetaryAmount FromString(string amount, string currency)
	    {
	        return new MonetaryAmount(double.Parse(amount, NumberStyles.Any), currency);
	    }


	    public static MonetaryAmount Convert(MonetaryAmount amount, string toConcurrency)
	    {
	        // TODO: This requires some conversion magic and is therefore broken
	        return new MonetaryAmount(amount.Value, toConcurrency);
	    }

	    #endregion

	}
}

namespace NHibernateInAction.DataBinding.Manual
{
	public class CreditCard : BillingDetails
	{
		public virtual string Number
		{
			get { return number; }
		}

		public virtual int ExpYear
		{
			get { return expYear; }
		}

		private string number;
		private int expYear;


		/// <summary> No-arg constructor for tools. </summary>
		internal CreditCard() : base()
		{
		}


		/// <summary> Full constructor. </summary>
		public CreditCard(string ownerName, string number, int expYear)
			: base(ownerName)
		{
			this.number = number;
			this.expYear = expYear;
		}


		// ********************** Common Methods ********************** //

		public override string ToString()
		{
			return "CreditCard of " + OwnerName + ", " + "Number: '" + Number + "', Expires: " + ExpYear;
		}
	}
}
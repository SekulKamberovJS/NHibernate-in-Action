
namespace NHibernateInAction.DataBinding.Manual
{
	public class BankAccount : BillingDetails
	{
		public virtual string Number
		{
			get { return number; }
			set { number = value; }
		}

		public virtual string BankName
		{
			get { return bankName; }
			set { bankName = value; }
		}

		private string number;
		private string bankName;


		/// <summary> No-arg constructor for tools. </summary>
		internal BankAccount() : base()
		{
		}


		/// <summary> Full constructor. </summary>
		public BankAccount(string ownerName, string number, string bankName)
			: base(ownerName)
		{
			this.number = number;
			this.bankName = bankName;
		}


		// ********************** Common Methods ********************** //

		public override string ToString()
		{
			return "BankAccount of " + OwnerName + ", " + "Number: '" + Number + "', BankName: " + BankName;
		}
	}
}
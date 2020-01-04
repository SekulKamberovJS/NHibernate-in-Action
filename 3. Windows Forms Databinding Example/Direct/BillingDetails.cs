
namespace NHibernateInAction.DataBinding.Direct
{
	public class BillingDetails
	{
		public virtual string OwnerName
		{
			get { return ownerName; }
			set { ownerName = value; }
		}

		public virtual System.DateTime Created
		{
			get { return created; }

		}

		private string ownerName;
		private System.DateTime created = System.DateTime.Now;


		/// <summary> No-arg constructor for tools. </summary>
		public BillingDetails()
		{
		}

		/// <summary> Full constructor. </summary>
		protected internal BillingDetails(string ownerName)
		{
			this.ownerName = ownerName;
		}


		// ********************** Common Methods ********************** //

		public override string ToString()
		{
			return "BankAccount of " + OwnerName + ", " + "Created: " + Created.ToString("u");
		}
	}
}
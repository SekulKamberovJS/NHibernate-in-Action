
namespace NHibernateInAction.DataBinding.Manual
{
	public abstract class BillingDetails
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
		internal BillingDetails()
		{
		}

		/// <summary> Full constructor. </summary>
		protected internal BillingDetails(string ownerName)
		{
			this.ownerName = ownerName;
		}
	}
}
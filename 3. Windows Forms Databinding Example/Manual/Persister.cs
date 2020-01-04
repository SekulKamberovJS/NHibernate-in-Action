using System.Collections.Generic;

namespace NHibernateInAction.DataBinding.Manual
{
	public static class Persister
	{
		public static IList<BillingDetails> GetMyBillingDetails()
		{
			// Load entities "from the database"
			IList<BillingDetails> details = new List<BillingDetails>();
			details.Add(new BankAccount("Pierre Henri Kuaté", "137", "My Bank"));
			details.Add(new CreditCard("John Smith", "3333333", 2007));
			details.Add(new CreditCard("A friend of mine", "007", 2099));
			details.Add(new BankAccount("John Doe", "777", "Unknown"));
			return details;
		}

		public static void Save(IList<BillingDetails> details)
		{
			// Open a "database connection"
			System.Text.StringBuilder database = new System.Text.StringBuilder();
			// "Save" the entities
			foreach (BillingDetails detail in details)
				database.AppendLine(detail.ToString());
			// "Commit"
			System.Windows.Forms.MessageBox.Show(database.ToString(), "Content of the database:");
		}
	}
}

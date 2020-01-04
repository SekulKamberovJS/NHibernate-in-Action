using System.Collections.Generic;

namespace NHibernateInAction.DataBinding.Manual
{
	public partial class ManageBillingDetailsForm : System.Windows.Forms.Form
	{
		IList<BillingDetails> _details = new List<BillingDetails>();

		public ManageBillingDetailsForm()
		{
			InitializeComponent();
		}

		public ManageBillingDetailsForm(IList<BillingDetails> details)
			: this()
		{
			_details = details;

			UpdateDisplayedList();
		}

		public IList<BillingDetails> GetMyBillingDetails()
		{
			return _details;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void UpdateDisplayedList()
		{
			listOfDetails.Items.Clear();
			foreach (BillingDetails details in _details)
			{
				System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
				item.Text = details.GetType().Name;
				item.SubItems.Add(details.OwnerName);
				if(details is BankAccount)
				{
					BankAccount account = details as BankAccount;
					item.SubItems.Add(account.Number);
					item.SubItems.Add(account.BankName);
				}
				else if(details is CreditCard)
				{
					CreditCard card = details as CreditCard;
					item.SubItems.Add(card.Number);
					item.SubItems.Add(card.ExpYear.ToString());
				}
				listOfDetails.Items.Add(item);
			}
		}

		private void btnAddAccount_Click(object sender, System.EventArgs e)
		{
			try
			{
				DoEdit( new BankAccount(), true);
				UpdateDisplayedList();
			}
			catch(System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString(), Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		private void btnAddCard_Click(object sender, System.EventArgs e)
		{
			try
			{
				DoEdit(null, true); // By convention we send null for new credit card
				UpdateDisplayedList();
			}
			catch(System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString(), Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(listOfDetails.SelectedItems.Count == 0)
			{
				System.Windows.Forms.MessageBox.Show("Select an item in the list first", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				return;
			}

			try
			{
				DoEdit( _details[listOfDetails.SelectedItems[0].Index], false );
				UpdateDisplayedList();
			}
			catch(System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString(), Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		private void listOfDetails_DoubleClick(object sender, System.EventArgs e)
		{
			if (listOfDetails.SelectedItems.Count > 0)
				listOfDetails.SelectedItems[0].Checked = !listOfDetails.SelectedItems[0].Checked;
			btnEdit_Click(sender, e);
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			for(int i=listOfDetails.Items.Count-1; i>=0; i--)
				if(listOfDetails.Items[i].Checked)
					_details.RemoveAt(i);
			UpdateDisplayedList();
		}

		private void DoEdit(BillingDetails details, bool add)
		{
			EditBillingDetailsForm form = new EditBillingDetailsForm(details);
			form.ShowDialog();
			details = form.GetMyBillingDetails(); // Perform copy from the GUI to the entity
			if(add)
				_details.Add(details);
		}
	}
}
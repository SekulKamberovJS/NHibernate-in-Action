
namespace NHibernateInAction.DataBinding.Manual
{
	public partial class EditBillingDetailsForm : System.Windows.Forms.Form
	{
		BillingDetails _details;

		public EditBillingDetailsForm()
		{
			InitializeComponent();
		}

		public EditBillingDetailsForm(BillingDetails details)
			: this()
		{
			_details = details;
			// Display
			if(_details != null)
				editOwnerName.Text = _details.OwnerName;
				//editOwnerName.DataBindings.Add("Text", details, "OwnerName");
			if(_details is BankAccount)
			{
				label3.Text = "Bank name";
				BankAccount account = details as BankAccount;
				editNumber.Text = account.Number;
				editBankOrExpYear.Text = account.BankName;
			}
			else
			{
				label3.Text = "Exp year";
				if(_details != null) // Convention for new credit cards
				{
					CreditCard card = details as CreditCard;
					editNumber.Text = card.Number;
					editBankOrExpYear.Text = card.ExpYear.ToString();
					editNumber.ReadOnly = editBankOrExpYear.ReadOnly = true;
				}
			}
		}

		public BillingDetails GetMyBillingDetails()
		{
			if (_details == null) // Convention for new credit cards
				_details = new CreditCard(editOwnerName.Text, editNumber.Text, int.Parse(editBankOrExpYear.Text));
			_details.OwnerName = editOwnerName.Text;
			if(_details is BankAccount)
			{
				BankAccount account = _details as BankAccount;
				account.Number = editNumber.Text;
				account.BankName = editBankOrExpYear.Text;
			}
			return _details;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
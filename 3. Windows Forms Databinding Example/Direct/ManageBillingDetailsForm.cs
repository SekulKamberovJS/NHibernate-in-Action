using System.Collections.Generic;

namespace NHibernateInAction.DataBinding.Direct
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

			/*System.Windows.Forms.BindingSource bindingSource = new System.Windows.Forms.BindingSource();
			bindingSource.DataSource = _details;
			dataGridView.DataSource = bindingSource;*/
			dataGridView.DataSource = _details;
		}

		public IList<BillingDetails> GetMyBillingDetails()
		{
			return _details;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
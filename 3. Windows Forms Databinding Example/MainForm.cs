
namespace NHibernateInAction.DataBinding
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void btnManual_Click(object sender, System.EventArgs e)
		{
			Manual.ManageBillingDetailsForm form = new Manual.ManageBillingDetailsForm(
				Manual.Persister.GetMyBillingDetails() );
			form.ShowDialog();
			Manual.Persister.Save( form.GetMyBillingDetails() );
		}

		private void btnDirect_Click(object sender, System.EventArgs e)
		{
			Direct.ManageBillingDetailsForm form = new Direct.ManageBillingDetailsForm(
				Direct.Persister.GetMyBillingDetails());
			form.ShowDialog();
			Direct.Persister.Save( form.GetMyBillingDetails() );
		}
	}
}
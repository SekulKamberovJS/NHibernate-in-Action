using NHibernateInAction.CaveatEmptor.Test;

namespace NHibernateInAction.CaveatEmptor
{
	static class Program
	{
		static void Main()
		{
			AllClassesSaveLoadFixture test = new AllClassesSaveLoadFixture();
			test.SetUpData();

			test.BeforeEachTest();
			test.Can_Save_Load_User();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_Item();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_Category();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_Comment();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_Bid();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_Customer();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_CategorizedItem();
			test.AfterEachTest();

			test.BeforeEachTest();
			test.Can_Save_Load_BillingDetails();
			test.AfterEachTest();
		}
	}
}

using System;
using Iesi.Collections;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Test.Utility;
using NUnit.Framework;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;
using NHibernate;


namespace NHibernateInAction.CaveatEmptor.Test
{
    [TestFixture]
	public class CategorizedItemFixture : TestFixtureWithSampleDataBase
	{
        [Test]
		public virtual void Composite_Key_Mapping_Works_For_CategorizedItems()
		{
			InitData();

			// Query for Category and all categorized Items (three tables joined)
			NHibernateHelper.BeginTransaction();
			ISession s = NHibernateHelper.Session;

			IQuery q = s.CreateQuery("select c from Category as c left join fetch c.CategorizedItems as ci join fetch ci.Item as i");
			System.Collections.ICollection result = new HashedSet(q.List());
			Assert.IsTrue(result.Count == 2);

			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			// Check initialization (should be eager fetched)
			foreach(Category cat in result)
			{
				foreach(CategorizedItem catItem in cat.CategorizedItems)
				{
					Assert.IsTrue(catItem != null);
				}
			}
		}

        [Test]
		public virtual void Can_Delete_CategorizedItems_From_Item()
		{
			InitData();

			// Delete all links for auctionFour by clearing collection
			NHibernateHelper.BeginTransaction();
			ISession s = NHibernateHelper.Session;
			Item i = (Item) s.Get(typeof(Item), auctionFour.Id);
			i.CategorizedItems.Clear();
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			// Check deletion
			NHibernateHelper.BeginTransaction();
			s = NHibernateHelper.Session;
			long tempAux = carsLuxury.Id;
			long tempAux2 = auctionFour.Id;
			CategorizedItem catItem = (CategorizedItem) s.Get(typeof(CategorizedItem), new CategorizedItemId(tempAux, tempAux2));
			Assert.IsTrue(catItem == null);
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();
		}

        [Test]
		public virtual void Can_Delete_CategorizedItems_From_Category()
		{
			InitData();

			// Delete all links for auctionFour by clearing collection
			NHibernateHelper.BeginTransaction();
			ISession s = NHibernateHelper.Session;
			Category c = (Category) s.Get(typeof(Category), carsSUV.Id);
			c.CategorizedItems.Clear();
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			// Check deletion
			NHibernateHelper.BeginTransaction();
			s = NHibernateHelper.Session;
			long tempAux = carsSUV.Id;
			long tempAux2 = auctionThree.Id;
			CategorizedItem catItem = (CategorizedItem) s.Get(typeof(CategorizedItem), new CategorizedItemId(tempAux, tempAux2));
			Assert.IsTrue(catItem == null);
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();
		}
	}
}
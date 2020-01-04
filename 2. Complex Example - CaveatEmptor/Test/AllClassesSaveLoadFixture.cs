using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using NHibernateInAction.CaveatEmptor.Dao;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Persistence;
using NHibernateInAction.CaveatEmptor.Test.Utility;
using NUnit.Framework;

namespace NHibernateInAction.CaveatEmptor.Test
{
    [TestFixture]
    public class AllClassesSaveLoadFixture : TestFixtureBase
	{
        #region Setup

        private static readonly ILog _log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        private ItemDAO items;
        private UserDAO users;
        private CommentDAO comments;
        private CategoryDAO categories;
            
        private User tobin;
        private Item iMac;

        public User CreateTobin()
        {
            User t = new User(
                "Tobin", 
                "Harris", 
                "tobinharris", 
                "I♥NHibernate", 
                "tobin@tobinharris.com"
                );

            t.HomeAddress = new Address("Monkbridge Road","LS6 4DX", "Leeds");
            t.BillingAddress = new Address("Round Foundry Media Center", "LS11 4QP", "Leeds");

            return t;
        }

        public Item CreateAppleiMac(User seller)
        {
            return new Item(
                "Apple iMac 2.4Ghz",
                "Used Apple iMac in great condition",
                seller,
                new MonetaryAmount(999.99, "GBP"),
                new MonetaryAmount(100.00, "GBP"),
                DateTime.Now,
                DateTime.Now.AddDays(5)
                );
        }

            

        [TestFixtureSetUp]
        public void SetUpData()
        {
            //create database
            CreateDatabase();
                
            //create common test data
            tobin = CreateTobin();
            iMac = CreateAppleiMac(tobin);

            //save them using DAOs
            items = new ItemDAO();
            users = new UserDAO();

            users.MakePersistent(tobin);
            items.MakePersistent(iMac);

            //commit
            NHibernateHelper.CommitTransaction();
            NHibernateHelper.CloseSession();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            //create DAOs
            items = new ItemDAO();
            users = new UserDAO();
            comments = new CommentDAO();
            categories = new CategoryDAO();
        }

        [TearDown]
        public void AfterEachTest()
        {
            //close session
            NHibernateHelper.RollbackTransaction();
        }

        #endregion
        
        [Test]
        public void Can_Save_Load_User()
        {
            //can find the common user we created at the  start
            Assert.AreEqual(1, users.FindAll().Count);
            Assert.AreEqual("Tobin", ((User)users.FindAll()[0]).Firstname);
        }

        [Test]
        public void Can_Save_Load_Item()
        {   
            //make sure the item find works
            Assert.AreEqual(1, items.FindAll().Count);
            
            //reload item and make sure resever price dehydrated ok
            Item item = items.GetById(iMac.Id);
            Assert.AreEqual(iMac.ReservePrice, item.ReservePrice);
        }

        [Test]
        public void Can_Save_Load_Category()
        {
            Category computers = new Category("Computer Equipment");
            computers.AddChildCategory(new Category("Apple"));
            computers.AddChildCategory(new Category("Dell"));
            categories.MakePersistent(computers);
            NHibernateHelper.CommitTransaction();
            NHibernateHelper.CloseSession();

        	IList<Category> rootCategories = categories.FindAll(true);
			Assert.AreEqual(1, rootCategories.Count);
            Assert.AreEqual(2, computers.ChildCategories.Count);
        }

        [Test]
        public void Can_Save_Load_Comment()
        {
            Comment comment = new Comment(Rating.Excellent,"This was an awesome item!",tobin,iMac);
            comments.MakePersistent(comment);

            Assert.AreEqual(1, comments.FindAll().Count);
        }

        [Test]
        public void Can_Save_Load_Bid()
        {
            Assert.AreEqual(1, items.FindAll().Count);
            
            Bid bid = new Bid(
                new MonetaryAmount(200, "GBP"), 
                iMac, 
                tobin);

            iMac.AddBid(bid);
            
            Assert.AreEqual(1, iMac.Bids.Count);
            Assert.AreEqual(1, items.FindAll().Count);

			NHibernateHelper.Session.Save(bid);
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			//test that can load the bid "manually" and that 
            //the .Item and .Bidder associations are set
            Bid loaded = NHibernateHelper.Session.Get<Bid>(bid.Id);
            Assert.IsNotNull(loaded, "No loaded bid found");
            Assert.IsTrue(iMac.Equals(loaded.Item),"Items not same");
            Assert.AreEqual(iMac, loaded.Item);
            
            Assert.IsTrue(tobin.Equals(loaded.Bidder), "Bidder not the same");
            Assert.AreEqual(tobin, loaded.Bidder);

            //test that we can reload the Item and that the Bids are restored also
            Item reloaded = items.GetById(iMac.Id);
            Assert.AreEqual(1, reloaded.Bids.Count);
            Assert.AreEqual(bid, reloaded.Bids[0]);
        }

        
        [Test]
        public void Can_Save_Load_Customer()
        {
            Customer customer = new Customer();
            customer.CustomerLocation = new CustomerLocation();
            customer.CustomerLocation.One = "United Kingdom";
            customer.CustomerLocation.Two = "North America";
            
            DAO<Customer> customerDAO = new DAO<Customer>();
			NHibernateHelper.Session.Save(customer.CustomerLocation); // Note: Can't cascade
            customerDAO.MakePersistent(customer);
            NHibernateHelper.Session.Clear();
            
            Customer reloaded = customerDAO.GetById(customer.Id);
            Assert.AreEqual(customer.Id, reloaded.Id);
            //TODO: Uncomment this assertion once Equals on customer is done
            //Assert.AreEqual(customer, reloaded);
            Assert.IsNotNull(reloaded.CustomerLocation);
            Assert.AreEqual("United Kingdom", reloaded.CustomerLocation.One);
        }

        [Test]
        public void Can_Save_Load_CategorizedItem()
        {
            Category myItems = new Category("My Items");
            categories.MakePersistent(myItems);
            CategorizedItem ci = new CategorizedItem(tobin.Username, myItems, iMac);
            
            //TODO: Remove explicit saving of categorized item in this test. This should be persisted by reachability?
            NHibernateHelper.Session.Save(ci);
            NHibernateHelper.CommitTransaction();

            Item reloaded = items.GetById(iMac.Id);
            Assert.AreEqual(1, reloaded.CategorizedItems.Count);
        }

        [Test]
        public void Can_Save_Load_BillingDetails()
        {
            BillingDetails cardDetails = new CreditCard("Tobin Harris", tobin, "111222333444",CreditCardType.MasterCard,"05","08");
            BillingDetails bankDetails = new BankAccount("Tobin Harris", tobin, "12345566", "HSBC", "X-Y-Z");

            tobin.AddBillingDetails(cardDetails);
            tobin.AddBillingDetails(bankDetails);
			Assert.AreEqual(2, tobin.BillingDetails.Count);

            users.MakePersistent(tobin);
        	NHibernateHelper.CommitTransaction();
            NHibernateHelper.CloseSession();

            User reloaded = users.GetById(tobin.Id);
			Assert.AreEqual(tobin, reloaded);
			Assert.AreEqual(2, reloaded.BillingDetails.Count);
			Assert.IsTrue(reloaded.BillingDetails.Contains(cardDetails));
			Assert.IsTrue(reloaded.BillingDetails.Contains(bankDetails));
        }
    }
}

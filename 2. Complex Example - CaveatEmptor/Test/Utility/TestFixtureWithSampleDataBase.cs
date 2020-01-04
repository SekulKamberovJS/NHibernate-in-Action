using System;
using NHibernateInAction.CaveatEmptor.Test.Utility;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;
using NHibernateInAction.CaveatEmptor.Dao;
using NHibernateInAction.CaveatEmptor.Model;


namespace NHibernateInAction.CaveatEmptor.Test.Utility
{
    /// <summary> No actual test, but only test data initialization. </summary>
    public abstract class TestFixtureWithSampleDataBase : TestFixtureBase
    {
        // Keep references to domain objects
        internal Category cars;
        internal Category carsLuxury;
        internal Category carsSUV;

        internal User u1;
        internal User u2;
        internal User u3;

        internal Item auctionOne;
        internal Item auctionTwo;
        internal Item auctionThree;
        internal Item auctionFour;

		

        /// <summary> Create test data for our domain model.
        /// 
        /// </summary>
        /// <throws>  Exception </throws>
        protected internal virtual void InitData()
        {
            CreateDatabase();

            // Prepare DAOS
            CategoryDAO catDAO = new CategoryDAO();
            UserDAO userDAO = new UserDAO();
            ItemDAO itemDAO = new ItemDAO();
            CommentDAO commentDAO = new CommentDAO();

            // Categories
            cars = new Category("Cars");
            carsLuxury = new Category("Luxury Cars");
            cars.AddChildCategory(carsLuxury);
            carsSUV = new Category("SUVs");
            cars.AddChildCategory(carsSUV);
            catDAO.MakePersistent(cars);

            // Users
            u1 = new User("Christian", "Bauer", "turin", "abc123", "christian@hibernate.org");
            u1.HomeAddress = new Address("Foo", "12345", "Bar");
            u1.IsAdmin = true;
            u2 = new User("Gavin", "King", "gavin", "abc123", "gavin@hibernate.org");
            u2.HomeAddress = new Address("Foo", "12345", "Bar");
            u3 = new User("Max", "Andersen", "max", "abc123", "max@hibernate.org");
            u3.HomeAddress = new Address("Foo", "12345", "Bar");
            userDAO.MakePersistent(u1);
            userDAO.MakePersistent(u2);
            userDAO.MakePersistent(u3);

            // BillingDetails
            BillingDetails ccOne = new CreditCard(
                "Christian  Bauer", u1, "1234567890",
                CreditCardType.MasterCard, "10", "2005");
            BillingDetails accOne = new BankAccount(
                "Christian Bauer", u1, "234234234234",
                "FooBar Rich Bank", "foobar123foobaz");
            u1.AddBillingDetails(ccOne);
            u1.AddBillingDetails(accOne);

            // Items
            DateTime tempAux = DateTime.Now;
            DateTime tempAux2 = DateTime.Now.AddDays(3);// inThreeDays
            auctionOne = new Item("Item One",
                                  "An item in the carsLuxury category.",
                                  u2,
                                  new MonetaryAmount(1.99, "USD"),
                                  new MonetaryAmount(50.33, "USD"),
                                  tempAux, tempAux2);
            auctionOne.SetPendingForApproval();
            auctionOne.Approve(u1);
            itemDAO.MakePersistent(auctionOne);
            new CategorizedItem(u1.Username, carsLuxury, auctionOne);

            DateTime tempAux3 = DateTime.Now;
            DateTime tempAux4 = DateTime.Now.AddDays(5); // inFiveDays
            auctionTwo = new Item("Item Two",
                                  "Another item in the carsLuxury category.",
                                  u2,
                                  new MonetaryAmount(2.22, "USD"),
                                  new MonetaryAmount(100.88, "USD"),
                                  tempAux3, tempAux4);
            itemDAO.MakePersistent(auctionTwo);
            new CategorizedItem(u1.Username, carsLuxury, auctionTwo);

            DateTime tempAux5 = DateTime.Now;
            DateTime tempAux6 = DateTime.Now.AddDays(3);// inThreeDays
            auctionThree = new Item("Item Three",
                                    "Don't drive SUVs.",
                                    u2,
                                    new MonetaryAmount(3.11, "USD"),
                                    new MonetaryAmount(300.55, "USD"),
                                    tempAux5, tempAux6);
            itemDAO.MakePersistent(auctionThree);
            new CategorizedItem(u1.Username, carsSUV, auctionThree);

            DateTime tempAux7 = DateTime.Now;
            DateTime tempAux8 = DateTime.Now.AddDays(7);// nextWeek
            auctionFour = new Item("Item Four",
                                   "Really, not even luxury SUVs.",
                                   u1,
                                   new MonetaryAmount(4.55, "USD"),
                                   new MonetaryAmount(40.99, "USD"),
                                   tempAux7, tempAux8);
            itemDAO.MakePersistent(auctionFour);
            new CategorizedItem(u1.Username, carsLuxury, auctionFour);
            new CategorizedItem(u1.Username, carsSUV, auctionFour);

            // Bids
            Model.Bid bidOne1 = new Model.Bid(new MonetaryAmount(12.12, "USD"), auctionOne, u3);
            Model.Bid bidOne2 = new Model.Bid(new MonetaryAmount(13.13, "USD"), auctionOne, u1);
            Model.Bid bidOne3 = new Model.Bid(new MonetaryAmount(14.14, "USD"), auctionOne, u3);

            auctionOne.AddBid(bidOne1);
            auctionOne.AddBid(bidOne2);
            auctionOne.AddBid(bidOne3);

            // Successful Bid
            auctionOne.SuccessfulBid = bidOne3;

            // Comments
            Comment commentOne = new Comment(Rating.Excellent, "This is Excellent.", u3, auctionOne);
            Comment commentTwo = new Comment(Rating.Low, "This is very Low.", u1, auctionThree);
            commentDAO.MakePersistent(commentOne);
            commentDAO.MakePersistent(commentTwo);

            NHibernateHelper.CommitTransaction();
            NHibernateHelper.CloseSession();
        }
    }
}
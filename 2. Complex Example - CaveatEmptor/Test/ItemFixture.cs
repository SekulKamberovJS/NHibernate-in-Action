using System;
using NHibernateInAction.CaveatEmptor.Dao;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Test.Utility;
using NUnit.Framework;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;


namespace NHibernateInAction.CaveatEmptor.Test
{
    [TestFixture]
	public class ItemFixture : TestFixtureWithSampleDataBase
	{
		

        [Test]
		public virtual void Can_Load_Item_And_Compare_Monetary_Amount()
		{
			InitData();

			ItemDAO itemDAO = new ItemDAO();

			long tempAux = auctionOne.Id;
			Item a1 = itemDAO.GetById(tempAux);
			Assert.AreEqual(a1.InitialPrice, new MonetaryAmount(1.99, "USD"));

			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();
		}


		
        [Test]
		public virtual void Can_Place_Bid_For_Item()
		{
			InitData();
            
			ItemDAO itemDAO = new ItemDAO();
			UserDAO userDAO = new UserDAO();

			long tempAux = auctionTwo.Id;
			Bid currentMaxBid = itemDAO.GetMaxBid(tempAux);
			long tempAux2 = auctionTwo.Id;
			Bid currentMinBid = itemDAO.GetMinBid(tempAux2);
			long tempAux3 = auctionTwo.Id;
			Item a2 = itemDAO.GetById(tempAux3, true);

			// Fail, auction is not active yet
			try
			{
				double bidAmount = 99.99;
				MonetaryAmount newAmount = new MonetaryAmount(bidAmount, "USD");
				long tempAux4 = u3.Id;
				a2.PlaceBid(userDAO.GetById(tempAux4), newAmount, currentMaxBid, currentMinBid);
			}
			catch(BusinessException success)
			{
			}

			// Fail, user isn't an admin
			try
			{
				a2.Approve(u3);
			}
			catch(PermissionException success)
			{
			}

			// Success, set active
			a2.SetPendingForApproval();
			a2.Approve(u1);

			// Success, place a bid
			try
			{
				double bidAmount = 100.00;
				MonetaryAmount newAmount = new MonetaryAmount(bidAmount, "USD");
				long tempAux5 = u3.Id;
				a2.PlaceBid(userDAO.GetById(tempAux5), newAmount, currentMaxBid, currentMinBid);
			}
			catch(BusinessException failure)
			{
				throw failure;
			}

			// Fail, bid amount is too low
			try
			{
				double bidAmount = 99.99;
				MonetaryAmount newAmount = new MonetaryAmount(bidAmount, "USD");
				long tempAux6 = u3.Id;
				a2.PlaceBid(userDAO.GetById(tempAux6), newAmount, currentMaxBid, currentMinBid);
			}
			catch(BusinessException success)
			{
			}
            
			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();
		}
	}
}
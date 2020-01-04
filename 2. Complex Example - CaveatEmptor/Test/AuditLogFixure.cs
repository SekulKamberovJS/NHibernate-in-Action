using System;
using NHibernate;
using NHibernateInAction.CaveatEmptor.Dao;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Test.Utility;
using NUnit.Framework;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;
using NHibernateInAction.CaveatEmptor.Persistence.Audit;


namespace NHibernateInAction.CaveatEmptor.Test
{
    [TestFixture]
	public class AuditLogFixure : TestFixtureBase
	{
        [Test]
		public void AuditLogInterceptor_Records_Log_Entry_When_Item_Persisted()
		{
			// Save a user without audit logging
			UserDAO userDAO = new UserDAO();
			User seller = new User("Christian", "Bauer", "turin", "abc123", "christian@NHibernate.org");
			userDAO.MakePersistent(seller);

			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			// Enable interceptor
			AuditLogInterceptor interceptor = new AuditLogInterceptor();
			NHibernateHelper.RegisterInterceptor(interceptor);
			interceptor.Session = NHibernateHelper.Session;
			interceptor.UserId = seller.Id;

			// Save an item with audit logging enabled
			Item item = new Item(
                "Warfdale Nearfield Monitors", 
                "Pair of 150W nearfield monitors for the home studio.", 
                seller,
				new MonetaryAmount(1.99, "USD"),
				new MonetaryAmount(50.33, "USD"),
                DateTime.Now,
                DateTime.Now.AddDays(1)
                );

			ItemDAO itemDAO = new ItemDAO();
			itemDAO.MakePersistent(item);

			// Synchronize state to trigger interceptor
			NHibernateHelper.Session.Flush();

			// Check audit log
            IQuery findAuditLogRecord = NHibernateHelper.Session.CreateQuery("from AuditLogRecord record where record.EntityId = :id");
			findAuditLogRecord.SetParameter("id", item.Id);
			AuditLogRecord foundRecord = findAuditLogRecord.UniqueResult<AuditLogRecord>();
			Assert.IsNotNull(foundRecord);
            Assert.AreEqual(foundRecord.UserId, seller.Id);

			NHibernateHelper.CommitTransaction();
			NHibernateHelper.CloseSession();

			// Deregister interceptor
			NHibernateHelper.RegisterInterceptor(null);
		}
	}
}
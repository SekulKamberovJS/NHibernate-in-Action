using NHibernateInAction.CaveatEmptor.Test.Utility;
using NUnit.Framework;

namespace NHibernateInAction.CaveatEmptor.Test.Utility
{
    /// <summary>
    /// Utility class for when you just want to insert some test data.
    /// </summary>
    [TestFixture, Explicit]
    public class TestDataFixture : TestFixtureWithSampleDataBase
    {
        [Test]
        public void Can_Init_Data_Without_Problems()
        {
            InitData();
        }
    }
}
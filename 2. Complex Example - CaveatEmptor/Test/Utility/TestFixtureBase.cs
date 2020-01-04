using System;
using NUnit.Framework;
using SchemaExport = NHibernate.Tool.hbm2ddl.SchemaExport;
using NHibernateHelper = NHibernateInAction.CaveatEmptor.Persistence.NHibernateHelper;


namespace NHibernateInAction.CaveatEmptor.Test.Utility
{
    [TestFixture]
    public abstract class TestFixtureBase 
    {
        
        public void CreateDatabase()
        {
            SchemaExport ddlExport = new SchemaExport(NHibernateHelper.Configuration);
            
            //if we want to actually look at the DDL generated, we can do this
            ddlExport.SetOutputFile("c:\\NHiA.sql");
            
            ddlExport.Create(true, true);
        }

        
        public void DropDatabase()
        {
            //closing the session makes sure it's not using the database
            //thus allowing the drop statements to run quickly
            NHibernateHelper.CloseSession();

            SchemaExport ddlExport = new SchemaExport(NHibernateHelper.Configuration);
            ddlExport.Drop(false, true);
        }
    }
}
using System;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor
{
    public class NHibernateCurrentSessionWebModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Application_BeginRequest;
            context.EndRequest += Application_EndRequest;
        }

        public void Dispose()
        {
        }

        #endregion

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            ISession session = NHibernateHelper.OpenSession();
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            ISession session = CurrentSessionContext.Unbind(
                NHibernateHelper.SessionFactory);
            if (session != null)
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    //Server.Transfer("...", true); // Error page
                }
                finally
                {
                    session.Close();
                }
        }
    }
}
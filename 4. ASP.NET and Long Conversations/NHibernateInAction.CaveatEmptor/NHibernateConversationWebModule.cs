using System;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor
{
    public class NHibernateConversationWebModule : IHttpModule
    {
        private const string EndOfConversationKey =
            "NHibernateInAction.CaveatEmptor.NHibernateConversationWebModule.EndOfConversation";

        private const string NHibernateSessionKey =
            "NHibernateInAction.CaveatEmptor.NHibernateConversationWebModule.NHibernateSession";

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += Application_PreRequestHandlerExecute;
            context.PostRequestHandlerExecute += Application_PostRequestHandlerExecute;
        }

        public void Dispose()
        {
        }

        #endregion

        public static void EndConversationAtTheEndOfThisRequest()
        {
            HttpContext.Current.Items[EndOfConversationKey] = true;
        }

        private void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            PerformRequest(true);
        }

        private void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            PerformRequest(false);
        }

        private void PerformRequest(bool begin)
        {
            try
            {
                if (begin)
                    OnRequestBeginning();
                else
                    OnRequestEnding();
            }
            catch (StaleObjectStateException staleEx)
            {
                // This implementation does not implement optimistic concurrency control!
                // Your application will not work until you add compensation actions!
                // Rollback, close everything, possibly compensate for any permanent changes
                // during the conversation, and finally restart business conversation. Maybe
                // give the user of the application a chance to merge some of his work with
                // fresh data... what you do here depends on your applications design.
                throw staleEx;
            }
            catch (Exception ex)
            {
                // Rollback only
                try
                {
                    if (CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory))
                        NHibernateHelper.GetCurrentSession().Transaction.Rollback();
                }
                catch
                {
                    // Could not rollback transaction after exception!
                }
                finally
                {
                    // Close the conversation and cleanup
                    HttpContext.Current.Session[NHibernateSessionKey] = null;
                    ISession currentSession = CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                    if (currentSession != null)
                        currentSession.Close();
                }
                // Let others handle it...
                throw new InfrastructureException(ex); // TODO: which "others" ?
            }
        }

        private void OnRequestBeginning()
        {
            // Continuing a conversation?
            var currentSession = (ISession) HttpContext.Current.Session[NHibernateSessionKey];
            if (currentSession == null)
            {
                // New conversation
                currentSession = NHibernateHelper.OpenSession();
                currentSession.FlushMode = FlushMode.Never;
            }
            CurrentSessionContext.Bind(currentSession);
            currentSession.BeginTransaction();
        }

        private void OnRequestEnding()
        {
            // Unbinding Session after processing
            ISession currentSession = CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            // End or continue the long-running conversation?
            if (HttpContext.Current.Items[EndOfConversationKey] != null)
            {
                currentSession.Flush();
                currentSession.Transaction.Commit();
                currentSession.Close();
                HttpContext.Current.Session[NHibernateSessionKey] = null;
            }
            else
            {
                currentSession.Transaction.Commit();
                HttpContext.Current.Session[NHibernateSessionKey] = currentSession;
            }
        }
    }
}
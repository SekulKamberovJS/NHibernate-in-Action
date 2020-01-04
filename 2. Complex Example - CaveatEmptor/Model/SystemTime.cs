////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary>
    /// SystemTime allows test cases to fake time. 
    /// This allows for repeatability when testing time based functionality.
    /// </summary>
    public class SystemTime
    {
        private static DateTime? FakeTime;

        /// <summary>
        /// Return the current time. If a call to Fake(...) has been made, that time will
        /// be returned.
        /// </summary>
        public static DateTime Now
        {
            get { return FakeTime.HasValue ? FakeTime.Value : DateTime.Now; }
        }

        /// <summary>
        /// Helper method used by domain claasses. 
        /// Some databases don't store the milliseconds, leading to problems where
        /// dates are used the "Equals" comparisons. There might be a better way?
        /// </summary>
        public static DateTime NowWithoutMilliseconds
        {
            get
            {
                DateTime now = Now;
                return new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second
                    );
            }
        }

        /// <summary>
        /// Make Now a fake time.
        /// </summary>
        /// <param name="fakeDateTime"></param>
        public static void Fake(DateTime fakeDateTime)
        {
            FakeTime = fakeDateTime;
        }
    }
}
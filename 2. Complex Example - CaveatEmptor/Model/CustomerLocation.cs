////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "CUSTOMER_LOCATION")]
    public class CustomerLocation
    {
        private string one;
        private string two;

        #region Common

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is CustomerLocation))
                return false;

            var customerLocation = (CustomerLocation) o;

            if (one != null ? one != customerLocation.one : customerLocation.one != null)
                return false;
            if (two != null ? two != customerLocation.two : customerLocation.two != null)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = (one != null ? one.GetHashCode() : 0);
            result = 29*result + (two != null ? two.GetHashCode() : 0);
            return result;
        }

        #endregion

        [CompositeId]
        [KeyProperty(1, Name = "One", Column = "CUSTOMER_LOCATION_ONE", Length = 100)]
        [KeyProperty(2, Name = "Two", Column = "CUSTOMER_LOCATION_TWO", Length = 100)]
        public virtual string One
        {
            get { return one; }
            set { one = value; }
        }

        public virtual string Two
        {
            get { return two; }
            set { two = value; }
        }
    }
}
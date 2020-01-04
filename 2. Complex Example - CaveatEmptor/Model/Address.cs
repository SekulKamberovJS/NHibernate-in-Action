////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> 
    /// The address of a User.
    /// An instance of this class is always associated with only
    /// one User and depends on that parent objects lifecycle,
    /// it is a component.
    /// </summary>
    [Serializable]
    [Component(Update = false)]
    public class Address
    {
        private string city;
        private string street;
        private string zipcode;

        /// <summary> No-arg constructor for tools.</summary>
        internal Address()
        {
        }

        /// <summary> Full constructor.</summary>
        public Address(string street, string zipcode, string city)
        {
            this.street = street;
            this.zipcode = zipcode;
            this.city = city;
        }

        #region Common Methods

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Address))
                return false;

            var address = (Address) o;

            if (city != address.city)
                return false;
            if (street != address.street)
                return false;
            if (zipcode != address.zipcode)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = street.GetHashCode();
            result = 29*result + zipcode.GetHashCode();
            result = 29*result + city.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "Street: '" + Street + "', " + "Zipcode: '" + Zipcode + "', " + "City: '" + City + "'";
        }

        #endregion

        [Property(Column = "STREET")]
        public virtual string Street
        {
            get { return street; }
            set { street = value; }
        }

        [Property(Column = "ZIP_CODE")]
        public virtual string Zipcode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }

        [Property(Column = "CITY")]
        public virtual string City
        {
            get { return city; }
            set { city = value; }
        }
    }
}
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
    /// This is the abstract superclass for BillingDetails.
    /// A BillingDetails object is always associated with a single
    /// User and depends on the lifecycle of that user. 
    /// It represents one of the billing strategies the User has choosen, usually
    /// on BillingDetails is the default in a collection of many.
    /// </summary>
    /// <remarks>
    /// A single BillingDetails object is a single bank account or
    /// credit card (or anything else that is a subclass). We expect
    /// these concrete classes (the superclass is abstract) to be very
    /// different, so we normalize the schema and use the
    /// "table-per-subclass" mapping strategy. This won't be a problem
    /// with polymorphic queries, as we only refer to billing information
    /// with the interface of the superclass (e.g. in the association from
    /// User).
    /// The CreditCard subclass is special, it has a typesafe enumeration
    /// object that represents the type of card (Amex, Visa, Mastercard, etc.)
    /// while the database has a SMALLINT column representing the card type.
    /// The mapping is done with a custom Hibernate UserType. The card type
    /// information is immutable and won't be updated.
    /// </remarks>
    [Serializable]
    [Class(Table = "BILLING_DETAILS")]
    public abstract class BillingDetails : IComparable
    {
        private readonly DateTime created = SystemTime.NowWithoutMilliseconds;
        private readonly User user;
        private long id;
        private string ownerName;
        private int version;

        /// <summary> 
        /// No-arg constructor for tools.
        /// </summary>
        protected BillingDetails()
        {
        }

        /// <summary>
        /// Full constructor
        /// </summary>
        protected internal BillingDetails(string ownerName, User user)
        {
            this.ownerName = ownerName;
            this.user = user;
        }

        #region Common Methods

        public virtual int CompareTo(object o)
        {
            // Billing Details are simply sorted by creation date
            if (o is BillingDetails)
                return Created.CompareTo(((BillingDetails) o).Created);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is BillingDetails))
                return false;

            var billingDetails = (BillingDetails) o;

            if (Created != billingDetails.Created)
                return false;
            if (OwnerName != billingDetails.OwnerName)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = Created.GetHashCode();
            result = 29*result + OwnerName.GetHashCode();
            return result;
        }

        #endregion

        [Id(0, Name = "Id", Column = "BILLING_DETAILS_ID", Access = "nosetter.camelcase")]
        [Generator(1, Class = "native")]
        public virtual long Id
        {
            get { return id; }
        }

        [Property(NotNull = true, Column = "OWNER_NAME")]
        public virtual string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }

        /// <remarks>
        /// Bidirectional, required as BillingDetails is USER_ID NOT NULL. This is also a read-only property that will never be updated.
        /// </remarks>
        [ManyToOne(Update = false, OuterJoin = OuterJoinStrategy.False, Column = "USER_ID",
            Access = "nosetter.camelcase")]
        public virtual User User
        {
            get { return user; }
        }

        /// <remarks>
        /// We can't change the creation time, so map it with update="false".
        /// </remarks>
        [Property(Update = false, NotNull = true, Column = "CREATED", Access = "nosetter.camelcase")]
        public virtual DateTime Created
        {
            get { return created; }
        }

        [Version(Column = "VERSION", Access = "nosetter.camelcase")]
        public virtual int Version
        {
            get { return version; }
        }

        /// <summary> 
        /// Checks if the billing information is correct.
        /// Check algorithm is implemented in subclasses.
        /// </summary>
        public abstract bool Valid { get; }
    }
}
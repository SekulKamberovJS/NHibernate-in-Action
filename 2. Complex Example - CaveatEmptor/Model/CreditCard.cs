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
    /// This billing strategy can handle various credit cards.
    /// The type of credit card is handled with a typesafe
    /// enumeration, CreditCardType.
    /// </summary>
    /// <seealso cref="CreditCardType">
    /// </seealso>
    [Serializable]
    // CreditCard subclass mapping to its own table, normalized. CreditCard is immutable, we map all properties with update="false"
    [JoinedSubclass(0, Table = "CREDIT_CARD", ExtendsType = typeof (BillingDetails))]
    [Key(2, Column = "CREDIT_CARD_ID", ForeignKey = "FK1_CREDIT_CARD_ID")]
    public class CreditCard : BillingDetails
    {
        private readonly string expMonth;
        private readonly string expYear;
        private readonly string number;
        private readonly CreditCardType type;

        /// <summary> No-arg constructor for tools.</summary>
        internal CreditCard()
        {
        }

        /// <summary> Full constructor. </summary>
        public CreditCard(string ownerName, User user, string number, CreditCardType type, string expMonth,
                          string expYear) : base(ownerName, user)
        {
            this.type = type;
            this.number = number;
            this.expMonth = expMonth;
            this.expYear = expYear;
        }

        #region Common

        public override string ToString()
        {
            return "CreditCard ('" + Id + "'), " + "Type: '" + Type + "'";
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is CreditCard))
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

        [Property(Update = false, NotNull = true, Column = "CC_TYPE", Access = "nosetter.camelcase")]
        public virtual CreditCardType Type
        {
            get { return type; }
        }

        [Property(NotNull = true, Column = "CC_NUMBER", Access = "nosetter.camelcase", Length = 16)]
        public virtual string Number
        {
            get { return number; }
        }

        [Property(Update = false, NotNull = true, Column = "EXP_MONTH", Access = "nosetter.camelcase", Length = 2)]
        public virtual string ExpMonth
        {
            get { return expMonth; }
        }

        [Property(Update = false, NotNull = true, Column = "EXP_YEAR", Access = "nosetter.camelcase", Length = 4)]
        public virtual string ExpYear
        {
            get { return expYear; }
        }

        public override bool Valid
        {
            get
            {
                // Use the type to validate the CreditCard details.
                // Implement your own syntactical validation of credit card information.
                return true; // Type.IsValid(this);
            }
        }
    }
}
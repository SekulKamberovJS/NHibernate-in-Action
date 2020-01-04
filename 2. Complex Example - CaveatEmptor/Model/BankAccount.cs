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
    /// This billing strategy uses a simple bank account. 
    /// </summary>
    [Serializable]
    //BankAccount subclass mapping to its own table, normalized.
    [JoinedSubclass(0, Table = "BANK_ACCOUNT", ExtendsType = typeof (BillingDetails))]
    [Key(2, Column = "BANK_ACCOUNT_ID", ForeignKey = "FK1_BANK_ACCOUNT_ID")]
    public class BankAccount : BillingDetails
    {
        private string bankName;
        private string bankSwift;
        private string number;

        /// <summary> 
        /// No-arg constructor for tools.
        /// </summary>
        internal BankAccount()
        {
        }

        /// <summary> 
        /// Full constructor. 
        /// </summary>
        public BankAccount(string ownerName, User user, string number, string bankName, string bankSwift)
            : base(ownerName, user)
        {
            this.number = number;
            this.bankName = bankName;
            this.bankSwift = bankSwift;
        }

        #region Common Methods

        public override string ToString()
        {
            return "BankAccount ('" + Id + "'), " + "Number: '" + Number + "'";
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is BankAccount))
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

        [Property(Column = "ACCOUNT_NUMBER", NotNull = true, Length = 16)]
        public virtual string Number
        {
            get { return number; }
            set { number = value; }
        }

        [Property(Column = "BANK_NAME", NotNull = true, Length = 255)]
        public virtual string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }

        [Property(Column = "BANK_SWIFT", NotNull = true, Length = 15)]
        public virtual string BankSwift
        {
            get { return bankSwift; }
            set { bankSwift = value; }
        }

        public override bool Valid
        {
            get
            {
                // TODO: Add code to validate bank account syntax here.
                return true;
            }
        }
    }
}
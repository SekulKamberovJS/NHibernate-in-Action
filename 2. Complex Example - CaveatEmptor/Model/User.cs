////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using Iesi.Collections.Generic;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Exceptions;

/*
A User is a versioned entity, with some special properties.
One is the username, it is immutable and unique. The
defaultBillingDetails property points to one of the
BillingDetails in the collection of all BillingDetails.

We never load any BillingDetails when a User is retrieved.
*/

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> A user of the CaveatEmptor auction application.
    /// 
    /// This class represents the user entity of CaveatEmptor business.
    /// The associations are: a Set of Items the user
    /// is selling, a Set of Bids the user has made,
    /// and an Address component. Also a Set of
    /// BuyNows, that is, immediate buys made for an item.
    /// 
    /// The BillingDetails are used to calculate and bill the
    /// user for his activities on our system. The username
    /// and password are used as login credentials. The
    /// ranking is a number that is increased by each successful
    /// transaction, but may also be manually increased (or decreased) by
    /// the system administrators.
    /// </summary>
    [Serializable]
    [Class(Table = "USERS")]
    public class User : IComparable
    {
        private readonly ISet<BillingDetails> billingDetails = new HashedSet<BillingDetails>();
        private readonly DateTime created = SystemTime.NowWithoutMilliseconds;
        private readonly ISet<Item> items = new HashedSet<Item>();
        private readonly string username;
        private string email;
        private string firstname;
        private Address homeAddress;
        private long id;
        private string lastname;
        private string password;
        private int version;

        /// <summary> No-arg constructor for tools.</summary>
        protected User()
        {
        }


        /// <summary> Full constructor.</summary>
        public User(string firstname, string lastname, string username, string password, string email, Address address,
                    ISet<Item> items, ISet<BillingDetails> billingDetails)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.username = username;
            this.password = password;
            this.email = email;
            homeAddress = address;
            this.items = items;
            this.billingDetails = billingDetails;
        }


        /// <summary> Simple constructor.</summary>
        public User(string firstname, string lastname, string username, string password, string email)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.username = username;
            this.password = password;
            this.email = email;
        }

        [Id(0, Name = "Id", Column = "USER_ID", Access = "nosetter.camelcase")]
        [Generator(1, Class = "native")]
        public virtual long Id
        {
            get { return id; }
        }

        [Version(Column = "VERSION", Access = "nosetter.camelcase")]
        public virtual int Version
        {
            get { return version; }
        }

        [Property(Column = "FIRST_NAME", NotNull = true)]
        public virtual string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        [Property(Column = "LAST_NAME", NotNull = true)]
        public virtual string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        [Property(Access = "nosetter.camelcase", Column = "USERNAME", /*Unique = true,*/ Update = false, NotNull = true,
            Length = 16)]
        public virtual string Username
        {
            get { return username; }
        }

        // Password is a keyword in some databases, so we quote it.
        [Property(Column = "`PASSWORD`", NotNull = true)]
        public virtual string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Property(Column = "EMAIL", NotNull = true)]
        public virtual string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Property(Column = "RANKING", NotNull = true)]
        public virtual int Ranking { get; set; }

        [ComponentProperty]
        public virtual Address HomeAddress
        {
            get { return homeAddress; }
            set { homeAddress = value; }
        }


        /// <summary>
        /// This is mapped by the BillingAddressComponent below
        /// </summary>
        public virtual Address BillingAddress { get; set; }

        //The default billing strategy, may be null if no BillingDetails exist.
        [ManyToOne(Column = "DEFAULT_BILLING_DETAILS_ID", NotNull = false, OuterJoin = OuterJoinStrategy.False,
            ForeignKey = "FK1_DEFAULT_BILLING_DETAILS_ID")]
        public virtual BillingDetails DefaultBillingDetails { get; set; }

        [Property(Column = "CREATED", Access = "nosetter.camelcase", Update = false, NotNull = true)]
        public virtual DateTime Created
        {
            get { return created; }
        }

        [Property(Column = "IS_ADMIN", NotNull = true)]
        public virtual bool IsAdmin { get; set; }


        [Set(0, Inverse = true, Cascade = CascadeStyle.None, Access = "nosetter.camelcase")]
        [Key(1)]
        [Column(2, Name = "SELLER_ID", NotNull = true)]
        [OneToMany(3, ClassType = typeof (Item))]
        public virtual ISet<Item> Items
        {
            get { return items; }
        }

        [Set(0, Inverse = true, Cascade = CascadeStyle.AllDeleteOrphan, Access = "nosetter.camelcase")]
        [Key(1)]
        [Column(2, Name = "USER_ID", NotNull = true)]
        [OneToMany(3, ClassType = typeof (BillingDetails))]
        public virtual ISet<BillingDetails> BillingDetails
        {
            get { return billingDetails; }
        }


        public virtual void AddItem(Item item)
        {
            if (item == null)
                throw new ArgumentException("Can't add a null Item.");
            Items.Add(item);
        }


        /// <summary> Adds a BillingDetails to the set.
        /// 
        /// This method checks if there is only one billing method
        /// in the set, then makes this the default.
        /// 
        /// </summary>
        public virtual void AddBillingDetails(BillingDetails billingDetails)
        {
            if (billingDetails == null)
                throw new ArgumentException("Can't add a null BillingDetails.");

            bool added = BillingDetails.Add(billingDetails);
            if (!added)
                throw new ArgumentException("Duplicates not allowed");

            if (BillingDetails.Count == 1)
            {
                DefaultBillingDetails = billingDetails;
            }
        }


        /// <summary> Removes a BillingDetails from the set.
        /// 
        /// This method checks if the removed is the default element,
        /// and will throw a BusinessException if there is more than one
        /// left to chose from. This might actually not be the best way
        /// to handle this situation.
        /// 
        /// </summary>
        public virtual void RemoveBillingDetails(BillingDetails billingDetails)
        {
            if (billingDetails == null)
                throw new ArgumentException("Can't remove a null BillingDetails.");

            if (BillingDetails.Count >= 2)
            {
                BillingDetails.Remove(billingDetails);
                DefaultBillingDetails = BillingDetails.GetEnumerator().Current;
            }
            else
            {
                throw new BusinessException("Please set new default BillingDetails first");
            }
        }

        #region Common Methods

        public virtual int CompareTo(object o)
        {
            if (o is User)
                return Created.CompareTo(((User) o).Created);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is User))
                return false;

            var user = (User) o;

            if (username != user.Username)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return username.GetHashCode();
        }


        public override string ToString()
        {
            return "User ('" + Id + "'), " + "Username: '" + Username + "'";
        }

        #endregion

        #region Business Methods

        public virtual void IncreaseRanking()
        {
            Ranking = Ranking + 1;
        }

        #endregion

        #region Nested type: BillingAddressComponent

        /// <summary>
        /// These class allow use of Mapping Attributes to work 
        /// with components. Not that this isn't a very elegant solution.
        /// </summary>
        [Component(Name = "BillingAddress")]
        private class BillingAddressComponent : Address
        {
            [Property(Column = "BILLING_CITY", Name = "City")] [Property(Column = "BILLING_STREET", Name = "Street")] [Property(Column = "BILLING_ZIP_CODE", Name = "Zipcode")] private string ingoreMe;
        }

        #endregion
    }
}
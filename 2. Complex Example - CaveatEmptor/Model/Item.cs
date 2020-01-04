////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using Iesi.Collections;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary>
    /// An item for sale.
    /// An Item is the central entity of an auction. One interesting
    /// aspect of this mapping is the bag used for the collection
    /// of Bids. The Item class uses an IList for this collection,
    /// and NHibernate will order the collection on load by the
    /// creation date of the bids.
    /// </summary>
    /// <remarks>
    /// An Item is the central entity of an auction. One interesting
    /// aspect of this mapping is the <bag> used for the collection
    /// of Bids. The Item Java class uses a List for this collection,
    /// and Hibernate will order the collection on load by the
    /// creation date of the bids.
    /// </remarks>
    [Serializable]
    [Class(Table = "ITEM", Lazy = false)]
    public class Item : IComparable, IAuditable
    {
        private readonly IList bids = new ArrayList();
        private readonly ISet categorizedItems = new HashedSet();
        private readonly DateTime endDate;
        private readonly MonetaryAmount initialPrice;
        private readonly string name;
        private readonly MonetaryAmount reservePrice;
        private readonly User seller;
        private readonly DateTime startDate;
        private DateTime? approvalDatetime;
        private User approvedBy;
        private DateTime created = SystemTime.NowWithoutMilliseconds;
        private string description;

        [Query(Name = "maxBid",
            Content =
                "select b from Bid b where b.Amount.Value = (select max(b.Amount.Value) from Bid b where b.Item.Id = :itemid)"
            )] private long id;

        private ItemState state;
        private Bid successfulBid;
        private int version;

        /// <summary> No-arg constructor for tools.</summary>
        protected Item()
        {
        }


        /// <summary> Full constructor.</summary>
        public Item(string name, string description, User seller, MonetaryAmount initialPrice,
                    MonetaryAmount reservePrice, DateTime startDate, DateTime endDate, ISet categories, IList bids,
                    Bid successfulBid)
        {
            this.name = name;
            this.seller = seller;
            this.description = description;
            this.initialPrice = initialPrice;
            this.reservePrice = reservePrice;
            this.startDate = startDate;
            this.endDate = endDate;
            categorizedItems = categories;
            this.bids = bids;
            this.successfulBid = successfulBid;
            state = ItemState.Draft;
        }


        /// <summary> Simple properties only constructor.</summary>
        public Item(string name, string description, User seller, MonetaryAmount initialPrice,
                    MonetaryAmount reservePrice, DateTime startDate, DateTime endDate)
        {
            this.name = name;
            this.seller = seller;
            this.description = description;
            this.initialPrice = initialPrice;
            this.reservePrice = reservePrice;
            this.startDate = startDate;
            this.endDate = endDate;
            state = ItemState.Draft;
        }

        [Version(Column = "VERSION", Access = "nosetter.camelcase")]
        public virtual int Version
        {
            get { return version; }
        }

        [Property(Column = "NAME", Length = 255, NotNull = true, Update = false, Access = "nosetter.camelcase")]
        public virtual string Name
        {
            get { return name; }
        }

        [Property(Column = "DESCRIPTION", Length = 4000, NotNull = true, Access = "nosetter.camelcase")]
        public virtual string Description // Limit to 4000 characters for Oracle
        {
            get { return description; }
            set { description = value; }
        }

        [Property(TypeType = typeof (MonetaryAmountCompositeUserType), Access = "nosetter.camelcase")]
        [Column(1, Name = "INITIAL_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "INITIAL_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount InitialPrice // UserType for prices, length is precision of decimal field for DDL
        {
            get { return initialPrice; }
        }

        [Property(TypeType = typeof (MonetaryAmountCompositeUserType), Access = "nosetter.camelcase")]
        [Column(1, Name = "RESERVE_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "RESERVE_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount ReservePrice // UserType for prices, length is precision of decimal field for DDL
        {
            get { return reservePrice; }
        }

        [Property(Column = "START_DATE", NotNull = true, Update = false, Access = "nosetter.camelcase")]
        public virtual DateTime StartDate
        {
            get { return startDate; }
        }

        [Property(Column = "END_DATE", NotNull = true, Update = false, Access = "nosetter.camelcase")]
        public virtual DateTime EndDate
        {
            get { return endDate; }
        }

        [Property(Column = "STATE", NotNull = true, Access = "nosetter.camelcase")]
        public virtual ItemState State
        {
            get { return state; }
        }

        [Property(Column = "APPROVAL_DATETIME", NotNull = false)]
        public virtual DateTime? ApprovalDatetime
        {
            get { return approvalDatetime; }
            set { approvalDatetime = value; }
        }

        [Property(Column = "CREATED", Update = false, NotNull = true, Access = "nosetter.camelcase")]
        public virtual DateTime Created
        {
            get { return created; }
        }

        [ManyToOne(Column = "APPROVED_BY_USER_ID", ForeignKey = "FK1_APPROVED_BY_USER_ID")]
        public virtual User ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        [ManyToOne(Column = "SELLER_ID", Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.False,
            Access = "nosetter.camelcase")]
        public virtual User Seller
        {
            get { return seller; }
        }

        [ManyToOne(Column = "SUCCESSFUL_BID_ID", OuterJoin = OuterJoinStrategy.False, NotNull = false)]
        public virtual Bid SuccessfulBid
        {
            get { return successfulBid; }
            set { successfulBid = value; }
        }

        /*We use a one-to-many association to express the relationship
	     to a set of categories. There is an intermediate entity class,
	     CategorizedItem, which in fact makes this a many-to-many
	     association between Item and Category.*/

        [Set(0, Access = "nosetter.camelcase", Inverse = true, Lazy = true, Cascade = CascadeStyle.AllDeleteOrphan)]
        [Key(1, ForeignKey = "FK2_CATEGORIZED_ITEM_ID")]
        [Column(2, Name = "ITEM_ID", NotNull = true, Length = 16)]
        [OneToMany(3, ClassType = typeof (CategorizedItem))]
        public virtual ISet CategorizedItems
        {
            get { return categorizedItems; }
        }

        [Bag(0, OrderBy = "CREATED desc", Access = "nosetter.camelcase", Inverse = true, Cascade = CascadeStyle.All,
            Name = "Bids")]
        [Key(1, Column = "ITEM_ID")]
        [OneToMany(2, ClassType = typeof (Bid))]
        public virtual IList Bids
        {
            get { return bids; }
        }

        #region IAuditable Members

        [Id(0, Name = "Id", Column = "ITEM_ID", Access = "nosetter.camelcase")]
        [Generator(1, Class = "native")]
        public virtual long Id
        {
            get { return id; }
        }

        #endregion

        /*[!-- TODO: This completely ignores currency --)]
[query(name="minBid")][![CDATA[
	select b from Bid b where b.amount.value =
        (select min(b.amount.value) from Bid b where b.item.id = :itemid)
]])][/query)]

[!-- TODO: This completely ignores currency --)]
[query(name="maxBid")][![CDATA[
	select b from Bid b where b.amount.value =
		(select max(b.amount.value) from Bid b where b.item.id = :itemid)
]])][/query)]*/


        public virtual void AddCategorizedItem(CategorizedItem catItem)
        {
            if (catItem == null)
                throw new ArgumentException("Can't add a null CategorizedItem.");
            CategorizedItems.Add(catItem);
        }


        public virtual void AddBid(Bid bid)
        {
            if (bid == null)
                throw new ArgumentException("Can't add a null Bid.");

            Bids.Add(bid);
        }

        #region Common Methods

        public virtual int CompareTo(object o)
        {
            if (o is Item)
            {
                return Created.CompareTo(((Item) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is Item))
                return false;

            var item = (Item) o;

            if (created != item.created)
                return false;

            if (name != null ? name != item.name : item.name != null)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = (name != null ? name.GetHashCode() : 0);
            result = 29*result + created.GetHashCode();
            return result;
        }


        public override string ToString()
        {
            return "Item ('" + Id + "'), " + "Name: '" + Name + "' " + "Initial Price: '" + InitialPrice + "'";
        }

        #endregion

        #region Business Methods

        /// <summary> Places a bid while checking business constraints.
        /// This method may throw a BusinessException if one of the requirements
        /// for the bid placement wasn't met, e.g. if the auction already ended.
        /// </summary>
        /// <param name="bidder">
        /// </param>
        /// <param name="bidAmount">
        /// </param>
        /// <param name="currentMaxBid"> the most valuable bid for this item
        /// </param>
        /// <param name="currentMinBid"> the least valuable bid for this item
        /// </param>
        /// <returns>
        /// </returns>
        public virtual Bid PlaceBid(User bidder, MonetaryAmount bidAmount, Bid currentMaxBid, Bid currentMinBid)
        {
            // Check highest bid (can also be a different Strategy (pattern))
            if (currentMaxBid != null && currentMaxBid.Amount.CompareTo(bidAmount) > 0)
            {
                throw new BusinessException("Bid too low.");
            }

            // Auction is active
            if (state != ItemState.Active)
                throw new BusinessException("Auction is not active yet.");

            // Auction still valid
            if (EndDate < DateTime.Now)
                throw new BusinessException("Can't place new bid, auction already ended.");

            // Create new Bid
            var newBid = new Bid(bidAmount, this, bidder);

            // Place bid for this Item
            AddBid(newBid);

            return newBid;
        }


        /// <summary> Anyone can set this item into PENDING state for approval.</summary>
        public virtual void SetPendingForApproval()
        {
            state = ItemState.Pending;
        }


        /// <summary> Approve this item for auction and set its state to active. </summary>
        public virtual void Approve(User byUser)
        {
            if (!byUser.IsAdmin)
                throw new PermissionException("Not an administrator.");

            if (state != ItemState.Pending)
                throw new BusinessException("Item still in draft.");

            state = ItemState.Active;
            approvedBy = byUser;
            approvalDatetime = SystemTime.NowWithoutMilliseconds;
        }

        #endregion
    }
}
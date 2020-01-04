using System;
using System.Collections;
using Iesi.Collections;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Exceptions;

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
    [Serializable]
    [Class(Table = "ITEM", Lazy = false)] // TODO: Lazy=true cause problem keeping values...
    public class Item : IComparable //, IAuditable
    {
        private readonly string name;
        //private User seller;
        private string description;
                       /*
		private double initialPrice;
		private double reservePrice;
		private DateTime startDate;
		private DateTime endDate;
		private ISet categorizedItems = new HashedSet();
		private IList bids = new ArrayList();
		private Bid successfulBid;
		private ItemState state;
		private User approvedBy;
		private DateTime? approvalDatetime;
		private DateTime created = DateTime.Now;*/

        private long id;
        private int version;


        /// <summary> No-arg constructor for tools.</summary>
        public Item()
        {
        }


        /// <summary> Full constructor.</summary>
        public Item(string name, string description, /*User seller, */double initialPrice, double reservePrice,
                    DateTime startDate, DateTime endDate, ISet categories, IList bids, Bid successfulBid)
        {
            this.name = name;
            //this.seller = seller;
            this.description = description;
                /*
			this.initialPrice = initialPrice;
			this.reservePrice = reservePrice;
			this.startDate = startDate;
			this.endDate = endDate;
			categorizedItems = categories;
			this.bids = bids;
			this.successfulBid = successfulBid;
			state = ItemState.Draft;*/
        }


        /// <summary> Simple properties only constructor.</summary>
        public Item(string name, string description, /*User seller, */double initialPrice, double reservePrice,
                    DateTime startDate, DateTime endDate)
        {
            this.name = name;
            //this.seller = seller;
            this.description = description;
                /*
			this.initialPrice = initialPrice;
			this.reservePrice = reservePrice;
			this.startDate = startDate;
			this.endDate = endDate;
			state = ItemState.Draft;*/
        }

        [Id(Name = "Id", Column = "ITEM_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id
        {
            get { return id; }
        }

        [Version(Column = "VERSION")]
        public virtual int Version
        {
            get { return version; }
        }

        [Property(Column = "NAME", Length = 255, NotNull = true, Update = false)]
        public virtual string Name
        {
            get { return name; }
        }

        [Property(Column = "DESCRIPTION", Length = 4000, NotNull = true)]
        public virtual string Description // Limit to 4000 characters for Oracle
        {
            get { return description; }
            set { description = value; }
        }

        #region IComparable Members

        public virtual int CompareTo(object o)
        {
            if (o is Item)
            {
//				return Created.CompareTo(((Item) o).Created);
            }
            return 0;
        }

        #endregion

        /*public virtual void AddCategorizedItem(CategorizedItem catItem)
		{
			if(catItem == null)
				throw new ArgumentException("Can't add a null CategorizedItem.");
			CategorizedItems.Add(catItem);
		}*/


        public virtual void AddBid(Bid bid)
        {
            if (bid == null)
                throw new ArgumentException("Can't add a null Bid.");
//			Bids.Add(bid);
        }


        // ********************** Common Methods ********************** //

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Item))
                return false;

            var item = (Item) o;

/*			if(created != item.created)
				return false;*/
            if (name != null ? name != item.name : item.name != null)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = (name != null ? name.GetHashCode() : 0);
//			result = 29*result + created.GetHashCode();
            return result;
        }


        public override string ToString()
        {
//			return "Item ('" + Id + "'), " + "Name: '" + Name + "' " + "Initial Price: '" + InitialPrice + "'";
            return "Item ('" + Id + "'), " + "Name: " + Name;
        }


        // ********************** Business Methods ********************** //

        /// <summary> Places a bid while checking business constraints.
        /// This method may throw a BusinessException if one of the requirements
        /// for the bid placement wasn't met, e.g. if the auction already ended.
        /// </summary>
        public virtual Bid PlaceBid( /*User bidder, */ double bidAmount, Bid currentMaxBid, Bid currentMinBid)
        {
            // Check highest bid (can also be a different Strategy (pattern))
            if (currentMaxBid != null && currentMaxBid.Amount.CompareTo(bidAmount) > 0)
            {
                throw new BusinessException("Bid too low.");
            }
/*
			// Auction is active
			if(state != ItemState.Active)
				throw new BusinessException("Auction is not active yet.");

			// Auction still valid
			if(EndDate < DateTime.Now)
				throw new BusinessException("Can't place new bid, auction already ended.");
*/
            // Create new Bid
            var newBid = new Bid(bidAmount, this /*, bidder*/);

            // Place bid for this Item
            AddBid(newBid);

            return newBid;
        }


        /// <summary> Anyone can set this item into PENDING state for approval.</summary>
        public virtual void SetPendingForApproval()
        {
//			state = ItemState.Pending;
        }


        /// <summary> Approve this item for auction and set its state to active. </summary>
        public virtual void Approve(User byUser)
        {
            if (!byUser.IsAdmin)
                throw new PermissionException("Not an administrator.");
/*
			if(state != ItemState.Pending)
				throw new BusinessException("Item still in draft.");

			state = ItemState.Active;
			approvedBy = byUser;
			approvalDatetime = DateTime.Now;*/
        }
    }
}
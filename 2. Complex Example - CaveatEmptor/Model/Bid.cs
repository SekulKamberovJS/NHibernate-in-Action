////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary>
    /// An immutable class representing one bid. 
    /// It is mapped as a component of item, hence we can't set "update"
    /// to false.
    /// </summary>
    /// <remarks>
    /// This class represents a single Bid for a particular Item. The mapping
    /// uses the MonetaryAmount and a UserType mapping to represent the
    /// monetary value of a bid, with a separate column for currency.
    /// 
    /// The relationship to the item is bidirectional and the outer join
    /// setting is "true", so loading a Bid will fetch the associated
    /// Item in the same select.
    /// 
    /// The same is true for the association to the User who made the bid,
    /// we usually need both the item and the bidder together with the bid.
    /// 
    /// Both the reference to Item and User are never updated, this is also
    /// true for all other properties of the Bid. A Bid is immutable. We also
    /// use a custom PropertyAccessor to avoid unused setter methods (the
    /// custom accessor only uses getter methods and sets fields directly).
    ////
    /// This class needs private setter methods for NHibernate.
    /// </remarks>
    [Class(Table = "BID")]
    [Serializable]
    public class Bid : IComparable
    {
        // Common id property.
        private readonly MonetaryAmount amount;
        private readonly User bidder;
        private readonly Item item;
        private DateTime created = SystemTime.NowWithoutMilliseconds;
        private long id;
        private int version;

        /// <summary> No-arg constructor for tools.</summary>
        protected Bid()
        {
        }

        /// <summary> Full constructor. </summary>
        public Bid(MonetaryAmount amount, Item item, User bidder)
        {
            this.amount = amount;
            this.item = item;
            this.bidder = bidder;
        }

        #region Common Methods

        public virtual int CompareTo(object o)
        {
            if (o is Bid)
            {
                return Created.CompareTo(((Bid) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is Bid))
                return false;

            var bid = (Bid) o;

            //TODO: Why isn't != working in Bid Equals()
            //Below, != doesn't work (I'm missing something I know)
            if (! Amount.Equals(bid.Amount))
                return false;

            if (Created != bid.Created)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = amount.GetHashCode();
            result = 29*result + created.GetHashCode();
            return result;
        }


        public override string ToString()
        {
            return "Bid ('" + Id + "'), " + "Created: '" + Created.ToString("r") + "' " + "Amount: '" + Amount + "'";
        }

        #endregion

        [Id(0, Name = "Id", Column = "BID_ID", Access = "nosetter.camelcase")]
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

        /// <remarks>
        /// UserType for prices, length is precision of decimal field for DDL.
        /// </remarks>
        [Property(Update = false, TypeType = typeof (MonetaryAmountCompositeUserType), Access = "nosetter.camelcase")]
        [Column(1, Name = "INITIAL_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "INITIAL_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount Amount
        {
            get { return amount; }
        }

        /// <remarks>
        /// The other side of this bidirectional one-to-many association to item.
        /// </remarks>
        [ManyToOne(Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.False, Access = "nosetter.camelcase",
            Column = "ITEM_ID")]
        public virtual Item Item
        {
            get { return item; }
        }

        /// <remarks> 
        /// The other side of this bidirectional one-to-many association to user.
        /// </remarks>
        [ManyToOne(Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.True, Access = "nosetter.camelcase",
            Column = "BIDDER_ID")]
        public virtual User Bidder
        {
            get { return bidder; }
        }

        /// <remarks>
        /// We can't change the creation time, so map it with update="false".
        /// </remarks>
        [Property(Update = false, NotNull = true, Access = "nosetter.camelcase", Column = "CREATED")]
        public virtual DateTime Created
        {
            get { return created; }
        }
    }
}
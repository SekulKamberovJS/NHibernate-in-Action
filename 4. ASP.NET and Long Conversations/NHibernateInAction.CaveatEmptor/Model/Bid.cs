using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> An immutable class representing one bid.
    /// Note: This class needs private setter methods for NHibernate.
    /// It is mapped as a component of item, hence we can't set "update"
    /// to false.
    /// </summary>
    /// <seealso cref="Item">
    /// </seealso>
    [Serializable]
// TODO:	[Class]
    public class Bid : IComparable
    {
        private readonly double amount;
        private readonly Item item;
        //private User bidder;
        private DateTime created = DateTime.Now;
        private long id;


        /// <summary> No-arg constructor for tools.</summary>
        internal Bid()
        {
        }


        /// <summary> Full constructor. </summary>
        public Bid(double amount, Item item /*, User bidder*/)
        {
            this.amount = amount;
            this.item = item;
            //this.bidder = bidder;
        }

        public virtual long Id
        {
            get { return id; }
        }

        public virtual double Amount
        {
            get { return amount; }
        }

        public virtual Item Item
        {
            get { return item; }
        }

        /*public virtual User Bidder
		{
			get { return bidder; }
		}*/

        public virtual DateTime Created
        {
            get { return created; }
        }

        #region IComparable Members

        public virtual int CompareTo(object o)
        {
            if (o is Bid)
            {
                return Created.CompareTo(((Bid) o).Created);
            }
            return 0;
        }

        #endregion

        // ********************** Common Methods ********************** //

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Bid))
                return false;

            var bid = (Bid) o;

            if (amount != bid.amount)
                return false;
            if (created != bid.created)
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


        // ********************** Business Methods ********************** //
    }
}
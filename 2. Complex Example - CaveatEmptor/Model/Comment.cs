////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> An immutable class representing a comment from users.
    /// Comments are always made in the context of an auction, that
    /// is, a comment has a reference to an Item. The comment has
    /// a free text field and a rating (using a typesafe enumeration).
    /// </summary>
    /// <remarks>
    /// A comment is mapped as an entity, with associations to
    /// Item and User. Both associations are eagerly fetched,
    /// as we usually need both objects to display a comment.
    /// All properties of a comment are immutable, except for
    /// the free text field. The comment text may be edited
    /// by system administrators. The text field limit of
    /// 4000 characters is basically an Oracle limit, so we
    /// might change it later to a real LONGTEXT type instead of a
    /// simple VARCHAR.
    /// </remarks>
    /// <seealso cref="Item">
    /// </seealso>
    /// <seealso cref="User">
    /// </seealso>
    /// <seealso cref="Rating">
    /// </seealso>
    [Serializable]
    [Class(Table = "COMMENTS")]
    public class Comment : IComparable
    {
        private readonly User fromUser;
        private readonly Item item;
        private readonly Rating rating;
        private DateTime created = SystemTime.NowWithoutMilliseconds;
        private long id;
        private string text;
        private int version;

        /// <summary>
        /// No-arg constructor for NHibernate/tools.
        /// </summary>
        internal Comment()
        {
        }

        /// <summary> 
        /// Full constructor. 
        /// </summary>
        public Comment(Rating rating, string text, User fromUser, Item item)
        {
            this.rating = rating;
            this.text = text;
            this.fromUser = fromUser;
            this.item = item;
        }

        #region Common Methods

        public virtual int CompareTo(object o)
        {
            if (o is Comment)
            {
                return Created.CompareTo(((Comment) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Comment))
                return false;

            var comment = (Comment) o;

            if (created != comment.created)
                return false;
            if (rating != comment.rating)
                return false;
            if (text != null ? text != comment.text : comment.text != null)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = rating.GetHashCode();
            result = 29*result + (text != null ? text.GetHashCode() : 0);
            result = 29*result + created.GetHashCode();
            return result;
        }


        public override string ToString()
        {
            return "Comment ('" + Id + "'), " + "Rating: '" + Rating + "'";
        }

        #endregion

        [Id(0, Name = "Id", Column = "COMMENT_ID", Access = "nosetter.camelcase")]
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

        // Simple property mapped with an enum
        [Property(Update = false, NotNull = true, Column = "RATING", Access = "nosetter.camelcase")]
        public virtual Rating Rating
        {
            get { return rating; }
        }

        [Property(Column = "COMMENT_TEXT", Length = 4000, Access = "nosetter.camelcase")]
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }

        [ManyToOne(Column = "FROM_USER_ID", OuterJoin = OuterJoinStrategy.True, Update = false, NotNull = true,
            Access = "nosetter.camelcase")]
        public virtual User FromUser
        {
            get { return fromUser; }
        }

        // A simple uni-directional one-to-many association to item.
        [ManyToOne(Column = "ITEM_ID", OuterJoin = OuterJoinStrategy.True, Update = false, NotNull = true,
            Access = "nosetter.camelcase")]
        public virtual Item Item
        {
            get { return item; }
        }

        [Property(Column = "CREATED", Access = "nosetter.camelcase")]
        public virtual DateTime Created
        {
            get { return created; }
        }
    }
}
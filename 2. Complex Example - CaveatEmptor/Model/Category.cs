////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;
using Iesi.Collections;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> 
    /// The CaveatEmptor Category can have child categories and each has Items.
    /// Categories can be nested, this is expressed as a bidirectional one-to-many
    /// relationship that references parent and child categories. Each Category
    /// can have many Items (and an Item can be in many categories). This is a
    /// true many-to-many relationship.
    /// 
    /// The optional class CategorizedItem can be used if additional
    /// information has to be kept about the link between a Category and an
    /// Item. The collection of items will then be mapped as a
    /// collection of dependent objects in the mapping for Category.
    /// </summary>
    /// <remarks>
    /// Categories are versioned and we make some use of the
    /// DirectSetAccessor for immutable properties and properties
    /// that are covered with convenience methods that protect
    /// entity relationships (setChildren() vs. addChildren()).
    ///
    /// Interesting is the bidirectional one-to-many association
    /// that references parent and child categories. This is a
    /// real parent/child association, with full cascading
    /// enabled.
    ///
    /// There can never be two categories with the same name at
    /// the same "level", that is, they can't have the same parent
    /// category. This is protected with a unique constraint.
    /// </remarks>
    /// <seealso cref="Item">
    /// </seealso>
    /// <seealso cref="CategorizedItem">
    /// </seealso>
    [Serializable]
    [Class(Table = "CATEGORY")]
    public class Category : IComparable
    {
        private readonly ISet categorizedItems = new HashedSet();
        private readonly ISet childCategories = new HashedSet();
        private DateTime created = SystemTime.NowWithoutMilliseconds;
        private long id;
        private string name;
        private Category parentCategory;
        private int version;
        
        /// <summary> No-arg constructor for tools.</summary>
        protected Category()
        {
        }

        /// <summary> Full constructor.</summary>
        public Category(string name, Category parentCategory, ISet childCategories, ISet categorizedItems)
        {
            this.name = name;
            this.parentCategory = parentCategory;
            this.childCategories = childCategories;
            this.categorizedItems = categorizedItems;
        }

        /// <summary> Simple constructor.</summary>
        public Category(string name)
        {
            this.name = name;
        }

        [Id(0, Name = "Id", Column = "CATEGORY_ID", Access = "nosetter.camelcase")]
        [Generator(1, Class = "native")]
        public virtual long Id
        {
            get { return id; }
        }

        [Property(0)]
        [Column(1, Name = "NAME", NotNull = true, Length = 255, UniqueKey = "UNIQUE_NAME_AT_LEVEL")]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        // Parent can be null for root categories.
        [ManyToOne(0, OuterJoin = OuterJoinStrategy.False, ForeignKey = "FK1_PARENT_CATEGORY_ID")]
        [Column(1, Name = "PARENT_CATEGORY_ID", NotNull = false)]
        public virtual Category ParentCategory
        {
            get { return parentCategory; }
            set { parentCategory = value; }
        }

        /// <remarks>
        /// We use a Set for this bidirectional one-to-many. Batch fetching is
		/// particulary interesting for  this association: We expect that the
		/// application will need much more childCategories if it accesses
		/// one. Batch fetching can significantly improve fetching of the
		/// whole category graph.
        /// </remarks>
        [Set(0, Cascade = CascadeStyle.AllDeleteOrphan, Inverse = true, BatchSize = 10, Access = "nosetter.camelcase")]
        [Key(1, Column = "PARENT_CATEGORY_ID")]
        [OneToMany(2, ClassType = typeof (Category))]
        public virtual ISet ChildCategories
        {
            get { return childCategories; }
        }

        /// <remarks>
        /// We use a one-to-many association to express the relationship
		/// to a set of items. There is an intermediate entity class,
		/// CategorizedItem, which in fact makes this a many-to-many
		/// association between Category and Item. 
        /// </remarks>
        [Set(0, Cascade = CascadeStyle.AllDeleteOrphan, Inverse = true, OuterJoin = OuterJoinStrategy.False,
            Access = "nosetter.camelcase")]
        [Key(1, ForeignKey = "FK1_CATEGORIZED_ITEM_ID")]
        [Column(2, Name = "CATEGORY_ID", NotNull = true, Length = 16)]
        [OneToMany(3, ClassType = typeof (CategorizedItem))]
        public virtual ISet CategorizedItems
        {
            get { return categorizedItems; }
        }

        [Property(Update = false, NotNull = true, Column = "CREATED", Access = "nosetter.camelcase")]
        public virtual DateTime Created
        {
            get { return created; }
        }
        public virtual void AddChildCategory(Category category)
        {
            if (category == null)
                throw new ArgumentException("Can't add a null Category as child.");
            // Remove from old parent category
            if (category.ParentCategory != null)
                category.ParentCategory.ChildCategories.Remove(category);
            // Set parent in child
            category.ParentCategory = this;
            // Set child in parent
            ChildCategories.Add(category);
        }

        public virtual void AddCategorizedItem(CategorizedItem catItem)
        {
            if (catItem == null)
                throw new ArgumentException("Can't add a null CategorizedItem.");
            CategorizedItems.Add(catItem);
        }

        #region Common

        public virtual int CompareTo(object o)
        {
            if (o is Category)
            {
                return String.CompareOrdinal(Name, ((Category) o).Name);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Category))
                return false;

            var category = (Category) o;

            if (created != category.created)
                return false;
            if (name != null ? name != category.name : category.name != null)
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
            return "Category ('" + Id + "'), " + "Name: '" + Name + "'";
        }

        #endregion
    }
}
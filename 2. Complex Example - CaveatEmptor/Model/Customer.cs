////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Class(Table = "CUSTOMER")]
    public class Customer
    {
        [Id(0, Name = "Id", Column = "CUSTOMER_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [ManyToOne(OuterJoin = OuterJoinStrategy.True, NotNull = false, Update = false)]
        [Column(1, Name = "CUSTOMER_LOC_ONE")]
        [Column(2, Name = "CUSTOMER_LOC_TWO")]
        public virtual CustomerLocation CustomerLocation { get; set; }

        //TODO: Implement Equals and GetHashCode for this Customer Class
    }
}
////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuat�
// January 2009
////////////////////////////////////////////////////////////////////////////////

namespace NHibernateInAction.CaveatEmptor.Model
{
    /// <summary> A marker interface for auditable persistent domain classes. </summary>
    public interface IAuditable
    {
        long Id { get; }
    }
}
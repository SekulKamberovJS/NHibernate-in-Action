////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    public enum ItemState
    {
        Draft,
        Pending,
        Active,
    }
}
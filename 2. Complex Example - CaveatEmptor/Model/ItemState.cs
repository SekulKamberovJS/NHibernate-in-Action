////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuat�
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
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
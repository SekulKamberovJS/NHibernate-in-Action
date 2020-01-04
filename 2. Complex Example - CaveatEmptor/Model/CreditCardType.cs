////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
	/// <summary> Enumeration for credit card types. </summary>
	[Serializable]
	public enum CreditCardType
	{
		MasterCard,
		Visa,
		Amex,
	}
}
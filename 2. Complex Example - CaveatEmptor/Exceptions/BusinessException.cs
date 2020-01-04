////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using System;

namespace NHibernateInAction.CaveatEmptor.Exceptions
{
	/// <summary> This exception is used to mark business rule violations. </summary>
	[Serializable]
	public class BusinessException : ApplicationException
	{
        //Expose a number of handy public constructors, 
        //that simply delegate to the base class.
		public BusinessException(){}
		public BusinessException(string message) : base(message){}
		public BusinessException(string message, Exception cause) : base(message, cause){}
		public BusinessException(Exception cause) : base("Business rule violation", cause){}
	}
}
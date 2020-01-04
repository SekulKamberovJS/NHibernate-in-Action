using System;

namespace NHibernateInAction.CaveatEmptor.Exceptions
{
    /// <summary> This exception is used to mark access violations. </summary>
    [Serializable]
    public class PermissionException : SystemException
    {
        public PermissionException()
        {
        }


        public PermissionException(string message) : base(message)
        {
        }


        public PermissionException(string message, Exception cause) : base(message, cause)
        {
        }


        public PermissionException(Exception cause) : base("Access violation", cause)
        {
        }
    }
}
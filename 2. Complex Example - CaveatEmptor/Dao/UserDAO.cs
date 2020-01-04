////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Author: Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
using User = NHibernateInAction.CaveatEmptor.Model.User;

namespace NHibernateInAction.CaveatEmptor.Dao
{
	/// <summary> 
	/// A typical DAO for User data access using NHibernate. 
	/// </summary>
	public class UserDAO : DAO<User>
	{
        // Insert any specialised user data access methods here.
	}
}
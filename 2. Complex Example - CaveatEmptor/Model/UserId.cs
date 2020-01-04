////////////////////////////////////////////////////////////////////////////////
// NHiberate In Action - Source Code
// Pierre Henri Kuaté
// January 2009
////////////////////////////////////////////////////////////////////////////////
namespace NHibernateInAction.CaveatEmptor.Model
{
    public class UserId
    {
        private string organizationId;
        private string username;


        public UserId(string username, string organizationId)
        {
            this.username = username;
            this.organizationId = organizationId;
        }

        public virtual string Username
        {
            get { return username; }
            set { username = value; }
        }

        public virtual string OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is UserId))
                return false;

            var userId = (UserId) o;

            if (organizationId != userId.organizationId)
                return false;
            if (username != userId.username)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = username.GetHashCode();
            result = 29*result + organizationId.GetHashCode();
            return result;
        }
    }
}
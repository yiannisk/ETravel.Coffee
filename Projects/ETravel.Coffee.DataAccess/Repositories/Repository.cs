using NHibernate;

namespace ETravel.Coffee.DataAccess.Repositories
{
    public class Repository
    {
        protected readonly ISession Session;

        /// <summary>
        /// With no arguments the repository constructor attempts to get a session from the SessionManager
        /// It does not close it however.
        /// </summary>
        public Repository()
        {
            Session = SessionManager.Instance.GetSession();
        }

        /// <summary>
        /// Access to the repository using a specified NHibernate Session
        /// </summary>
        /// <param name="session">The NHibernate Session to be used</param>
        public Repository(ISession session)
        {
            Session = session;
        }
    }
}

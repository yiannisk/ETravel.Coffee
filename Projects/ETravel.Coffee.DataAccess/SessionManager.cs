using System;
using System.Runtime.Remoting.Messaging;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using log4net;

namespace ETravel.Coffee.DataAccess
{
    //Borrowed paradigm from http://lostechies.com/nelsonmontalvo/2007/03/30/simple-nhibernate-example-part-4-session-management/
    /// <summary>
    /// This is the main class used for data access. It manages and provides access to NHibernate sessions. When handling web a web request
    /// sessions are stored in the HttpContext to be reused. Otherwise they are kept in the CallContext
    /// </summary>
    public sealed class SessionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionManager));

        /// <summary>
        /// The lazy loaded instance of the Singleton SessionManager
        /// </summary>
        public static SessionManager Instance { get { return Nested.Instance; } }

        private readonly ISessionFactory _sessionFactory;

        private static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() {}

            internal static readonly SessionManager Instance = new SessionManager();
        }

        private SessionManager()
        {
            _sessionFactory = BuildSessionFactory();
        }

        /// <summary>
        /// If no session exists for the current context it
        /// creates a new NHibernate Session and adds it to the HttpContext if it exists or to the CallContext otherwise.
        /// remember: Sessions are not thread safe.
        /// </summary>
        /// <returns>Returns the existing or newly created ISession associated with the http or call context</returns>
        public ISession GetSession()
        {
            ISession session = ThreadSession;

            if (session == null)
            {
                session = _sessionFactory.OpenSession();
                ThreadSession = session;
            }
            return ThreadSession;
        }

        public IStatelessSession OpenStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession();
        }

        public ISession NewSession()
        {
            return _sessionFactory.OpenSession();
        }

        /// <summary>
        /// Used to close the session once it is no longer needed
        /// </summary>
        public void CloseSession()
        {
            ISession session = ThreadSession;
            ThreadSession = null;

            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        //Configure The session factory
        private static ISessionFactory BuildSessionFactory()
        {
            try
            {
                return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2005.ConnectionString(Properties.Settings.Default.Database))
                    .Cache(c => c.UseQueryCache().UseSecondLevelCache().ProviderClass<NHibernate.Caches.SysCache.SysCacheProvider>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionManager>())
                .BuildSessionFactory();

            }
            catch (Exception e)
            {
                Log.Fatal("Could not get ISessionFactory!", e);
                throw;
            }
        }

        /// <summary>
        /// Optionally one can manually start and stop transactions. If this is not done then every statement is executed in its own transaction
        /// </summary>
        public void BeginTransaction()
        {
            ITransaction transaction = ThreadTransaction;

            if (transaction == null || !transaction.IsActive)
            {
                GetSession().BeginTransaction();
            }
        }

        /// <summary>
        /// Commits all changes done during the transaction
        /// </summary>
        public void CommitTransaction()
        {
            ITransaction transaction = ThreadTransaction;

            try
            {
                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Commit();
                }
            }
            catch (HibernateException ex)
            {
                Log.Error("Rolling back transaction ", ex);
                RollbackTransaction();
            }
        }

        public void RollbackTransaction()
        {
            ITransaction transaction = ThreadTransaction;

            try
            {
                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Rollback();
                }
            }
            catch (HibernateException ex)
            {
                Log.Error("Could not roll back the transaction", ex);
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref=”HttpContext” /> instead of the WinForms
        /// specific <see cref=”CallContext” />.  Discussion concerning this found at
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ITransaction ThreadTransaction
        {
            get
            {
                if (InWebContext())
                {
                    return ThreadSession != null ? ThreadSession.Transaction : null;
                }
                return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref=”HttpContext” /> instead of the WinForms
        /// specific <see cref=”CallContext” />.  Discussion concerning this found at
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private static ISession ThreadSession
        {
            get
            {
                if (InWebContext())
                {
                    return (ISession) HttpContext.Current.Items[SESSION_KEY];
                }
                return (ISession)CallContext.GetData(SESSION_KEY);
            }
            set
            {
                if (InWebContext())
                {
                    HttpContext.Current.Items[SESSION_KEY] = value;
                }
                CallContext.SetData(SESSION_KEY, value);
            }
        }

        private static bool InWebContext()
        {
            return HttpContext.Current != null;
        }

        private const string SESSION_KEY = "ETravel.Meandros.DataAccess.UoW.Session";
        private const string TRANSACTION_KEY = "ETravel.Meandros.DataAccess.UoW.Transaction";
    }
}

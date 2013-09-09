using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PEDevTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDevTracker.Controllers
{
    public class HibernateModule: IHttpModule
    {
        #region Attributes
        /// <summary>
        /// A key used in HttpContext.Current.Items to store the current configuration
        /// </summary>
        public static readonly string SessionContextKey = "HibernateSession";
        /// <summary>
        /// A key in HttpContext.Current.Application to store the factory
        /// </summary>
        public static readonly string FactoryContextKey = "HibernateFactory";
        private static FluentConfiguration configuration;
        #endregion
        #region Methods
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(Application_EndRequest);
            context.BeginRequest += new EventHandler(Application_BeginRequest);
        }
        /// <summary>
        /// Called at the begining of the request. We create the session object and store it in the context.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            context.Items[SessionContextKey] = HibernateModule.CreateSession();
        }

        /// <summary>
        /// Called at the end of the request. We flush the session object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            ISession session = context.Items[SessionContextKey] as ISession;
            if (session != null)
            {
                try
                {
                    session.Flush();
                    session.Close();
                }
                catch { //TODO 
                }
            }

            context.Items[SessionContextKey] = null;
        }

        public void Dispose()
        {
        }
        public static FluentConfiguration GetConfiguration()
        {
            if (configuration == null)
            {
                configuration = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.Dialect("NHibernate.Dialect.MsSql2008Dialect")
                    .ConnectionString(c =>
                        c.FromConnectionStringWithKey("PEDTConnString"))
                    .ShowSql())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DevPost>()/*.ExportTo(@"C:\Users\Sidoine\Documents\GitHub\Folke\Folke\temp_images")*/)
                    .ExposeConfiguration(BuildSchema);
            }
            return configuration;
        }
        
        /// <summary>
        /// Creates a new factory for the current session.
        /// </summary>
        /// <returns></returns>
        private static ISessionFactory CreateSessionFactory()
        {
            return GetConfiguration().BuildSessionFactory();
        }

        /// <summary>
        /// Retrieves the current Factory. Store it into HttpContext.Current.Application.
        /// </summary>
        public static ISessionFactory CurrentFactory
        {
            get
            {
                HttpContext currentContext = HttpContext.Current;
                ISessionFactory factory = currentContext.Application[FactoryContextKey] as ISessionFactory;

                if (factory == null)
                {
                    factory = HibernateModule.CreateSessionFactory();
                    currentContext.Application[FactoryContextKey] = factory;
                }

                return factory;
            }
        }

        /// <summary>
        /// Creates the Session using the current Factory.
        /// </summary>
        /// <returns></returns>
        public static ISession CreateSession()
        {
            return HibernateModule.CurrentFactory.OpenSession();
        }
        /// <summary>
        /// Builds the schema 
        /// </summary>
        /// <param name="config"></param>
        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }
        #endregion
    }
}
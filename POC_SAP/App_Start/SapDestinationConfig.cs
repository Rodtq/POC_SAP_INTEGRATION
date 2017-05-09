using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace POC_SAP
{
    public class SapDestinationConfig : IDestinationConfiguration
    {
        /// <summary>
        /// event from IDestinationConfiguration 
        /// </summary>
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
        /// <summary>
        /// Implementation from IDestinationConfiguration
        /// </summary>
        /// <returns>false</returns>
        public bool ChangeEventsSupported()
        {
            return false;
        }
        /// <summary>
        /// Get configured parameters.Implementation from IDestinationConfiguration
        /// </summary>
        /// <param name="destinationName">the SAP end point you want configured on webConfig at appSettings session </param>
        /// <returns>rfc parameters</returns>
        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters rfcParameters = new RfcConfigParameters();
            rfcParameters.Add(RfcConfigParameters.Name, destinationName);
            rfcParameters.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"]);
            rfcParameters.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]);
            rfcParameters.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            rfcParameters.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"]);
            rfcParameters.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"]);
            rfcParameters.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]);
            rfcParameters.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]);
            return rfcParameters;
        }

        public static void RegisterDestinations()
        {
            string destinationConfigName = ConfigurationManager.AppSettings["SAP_NAME"];
            bool destinationInitialised = false;
            if (!destinationInitialised)
            {
                IDestinationConfiguration destinationConfig = new SapDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);

                try
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationInitialised = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
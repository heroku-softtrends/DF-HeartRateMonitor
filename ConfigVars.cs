using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamforceIOTCloudApp
{
    public sealed class ConfigVars
    {
        public string DeviceID = string.Empty;
        public string DeviceToken = string.Empty;
        public string EnpointUrl = string.Empty;
        public string IronMQUrl = string.Empty;
        public string IronMQProjectID = string.Empty;
        public string IronMQToken = string.Empty;
        private ConfigVars()
        {
            DeviceID = Environment.GetEnvironmentVariable("deviceID");
            DeviceToken = Environment.GetEnvironmentVariable("deviceToken");
            EnpointUrl = Environment.GetEnvironmentVariable("endpointURL");
            IronMQUrl = Environment.GetEnvironmentVariable("queueURL");
            IronMQProjectID = Environment.GetEnvironmentVariable("IRON_MQ_PROJECT_ID");
            IronMQToken = Environment.GetEnvironmentVariable("IRON_MQ_TOKEN");
        }
        public static ConfigVars Instance { get { return ConfigVarInstance.Instance; } }

        private class ConfigVarInstance
        {
            static ConfigVarInstance()
            {
            }

            internal static readonly ConfigVars Instance = new ConfigVars();
        }
    }
}

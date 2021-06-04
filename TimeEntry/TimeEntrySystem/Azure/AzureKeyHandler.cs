using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Azure
{
    public static class KeyHandler
    {
        public static SecretClient GetClient()
        {
            var cred = new ClientSecretCredential(ConfigurationManager.AppSettings["tenant"], ConfigurationManager.AppSettings["clientId"], ConfigurationManager.AppSettings["clientSecret"]);
            var Client = new SecretClient(new Uri(ConfigurationManager.AppSettings["baseUrl"]), cred);
            return Client;
        }
    }
}
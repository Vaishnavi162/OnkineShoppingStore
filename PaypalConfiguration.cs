using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace OnlineShopingStore
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AZs4ywT3LdJFEPPMTWQ6NcVXObH-Bc8qyYW04x78phOBAE_IGcBegs4PRVmfNjkh5Shst0ga9tY0ZXnL";
            clientSecret = "EOOYDTWOOu_orfq3JxYHq3-ycGVvm3bQYqyvuOgR-i1GKHGzKpjv8vqdNK5LqMGLpuxPSwa3R7enz_H-";
        }

        private static Dictionary<string,string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();   
        }
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = getconfig();
            return apiContext;
        }
    }
}
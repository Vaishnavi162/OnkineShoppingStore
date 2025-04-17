using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace OnlineShopingStore
{
    public static class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }



    //public static class PaypalConfiguration
    //{
    //    public readonly static string clientId;
    //    public readonly static string clientSecret;

    //    static PaypalConfiguration()
    //    {
    //        var config = getconfig();
    //        clientId = "AenSNnozdb4ijzGsXNkAa7P9yhpUdSxQtVMKj7LiZ1ibpuRgJf1hWUVVia8OPMhwl88uk8-TdXxJdj_x";
    //        clientSecret = "EOnvqEGEf53B1TjyBxB3xcJivM793z6lYTYouAScHmsvzcmG_0R03-KJsli42gwJLpv1oHocIxawV_-n";
    //    }

    //    private static Dictionary<string, string> getconfig()
    //    {
    //        return PayPal.Api.ConfigManager.Instance.GetProperties();
    //    }
    //    private static string GetAccessToken()
    //    {
    //        string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
    //        return accessToken;
    //    }
    //    public static APIContext GetAPIContext()
    //    {
    //        APIContext apiContext = new APIContext(GetAccessToken());
    //        apiContext.Config = getconfig();
    //        return apiContext;
    //    }
    //}
}


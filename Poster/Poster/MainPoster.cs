using System;
using System.Configuration;
using TweetSharp;
using LinqToTwitter;
using System.Threading.Tasks;
using System.Net;

namespace Poster
{
    class MainPoster
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {

                AppSettingsReader cfgReader = new AppSettingsReader();

                string _UseProxy = cfgReader.GetValue("UseProxy", typeof(string)).ToString();

                var auth = new SingleUserAuthorizer
                {
                    CredentialStore = new SingleUserInMemoryCredentialStore
                    {
                        ConsumerKey = cfgReader.GetValue("ConsumerKey", typeof(string)).ToString(),
                        ConsumerSecret = cfgReader.GetValue("ConsumerSecret", typeof(string)).ToString(),
                        AccessToken = cfgReader.GetValue("AccessToken", typeof(string)).ToString(),
                        AccessTokenSecret = cfgReader.GetValue("AccessSecret", typeof(string)).ToString()
                    }
                };

                if (_UseProxy.Equals("1"))
                {
                    auth.Proxy = new WebProxy(cfgReader.GetValue("ProxyAddress", typeof(string)).ToString());
                    auth.Proxy.Credentials = new NetworkCredential(
                        cfgReader.GetValue("ProxyUser", typeof(string)).ToString(),
                        cfgReader.GetValue("ProxyPass", typeof(string)).ToString());
                }

                var twitterContext = new TwitterContext(auth);

                string _teste = "Tweet sent via LinqToTwitter";

                var tweet = await twitterContext.TweetAsync(_teste);

                if (tweet != null)
                {
                    Console.WriteLine("Status: {0}", tweet.StatusID.ToString());
                }

            }).Wait();

            Console.ReadLine();

        }
    }
}

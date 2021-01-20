using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SetMyBrainChartTest
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void GetAccessTokenTest()
        {
            string full_url = "https://www.facebook.com/connect/login_success.html#access_token=EAACNszpCnnsBAIvGMStbTnfolCjDdlgVaAZCJs1ZBTCGpgYlZC1KJA0r5R4ZANwZCLGLNlBEbSDgh723yzeo3MWtrCr0PFtSMIxTzjFKvdsYXcYEeZCIAZAGcrzeHyPsZAY9LtZA9n5KBRqVcoOQZAuwweJmZCSadE36TOkSsAq8OseZBAZDZD&data_access_expiration_time=1618769842&expires_in=5180942";
            Uri uri = new Uri(full_url);
            string fragment = uri.Fragment;
            fragment = fragment.Substring(1);
            NameValueCollection coll=HttpUtility.ParseQueryString(fragment);
            string access_token = coll["access_token"];
        }

        [TestMethod]
        public void JArrayTest()
        {
            string _response="{\"name\": \"Stefano D'Urso\",\"id\": \"3507795649343357\"}";
            JObject obj = JObject.Parse(_response);
            string user_id = obj.Value<string>("id");
        }

        [TestMethod]
        public void DeleteAccessToken()
        {
            string url = "http://graph.facebook.com/v9.0/3507795649343357/permissions?access_token=EAACNszpCnnsBAEpkpxrmgCNaLcu0cZCKhx76A49jJCZByZCiuBvQ7rd6SkZBnUfPNhL3B8zQ1I3sx5vyTib1JZAlxWhqF7dNTueuiXtxZCL6UMUjElwEKBKNlQSfCAimyeRoYZCbsxtj57OdIkStIfIGrkSyMix9OdCSalsVfCteHRGeP3gZAwJcaD9gaZBawVKsTVsLR6T8TAQZDZD";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string _response = reader.ReadToEnd();
                    }
                }
            }


            //request.Method = "DELETE";
            //request.ContentType = "application/x-www-form-urlencoded";
            //string postData = "debug=all&format=json&method=delete&pretty=0&suppress_http_code=1&transport=cors";
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //request.ContentLength = byteArray.Length;

            //using (Stream dataStream = request.GetRequestStream())
            //{
            //    dataStream.Write(byteArray, 0, byteArray.Length);
            //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //    {
            //        using (Stream stream = response.GetResponseStream())
            //        {
            //            using (StreamReader reader = new StreamReader(stream))
            //            {
            //                string _response = reader.ReadToEnd();
                            
            //            }
            //        }
            //    }
            //}
        }
    }
}

using EmployeeHub.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHub.Controller
{
    public class EmployeeController
    {
        public Task<HttpResponseMessage> GetCall(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = APIUrls.baseURI + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["apiToken"]);
                    var response = client.GetAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// creates a new employee
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostCall<T>(string url, T model) where T : class
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //string apiUrl = APIUrls.baseURI + url;
                //using (HttpClient client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(apiUrl);
                //    client.Timeout = TimeSpan.FromSeconds(900);
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //    var response = client.PostAsJsonAsync(apiUrl, model);
                //    response.Wait();
                //    return response;
                //}
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, APIUrls.baseURI + url))
                using (var httpContent = CreateHttpContent(model))
                {
                    string apiUrl = APIUrls.baseURI + url;
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["apiToken"]);
                    var response = client.PostAsync(apiUrl, httpContent);
                    response.Wait();
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        ///// <summary>
        ///// Updates emplyees details
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="url"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static Task<HttpResponseMessage> PutCall<T>(string url, T model) where T : class
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        string apiUrl = API_URIs.baseURI + url;
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.Timeout = TimeSpan.FromSeconds(900);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = client.PutAsJsonAsync(apiUrl, model);
        //            response.Wait();
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Delete employee's record
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static Task<HttpResponseMessage> DeleteCall(string url)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        string apiUrl = API_URIs.baseURI + url;
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.Timeout = TimeSpan.FromSeconds(900);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = client.DeleteAsync(apiUrl);
        //            response.Wait();
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}

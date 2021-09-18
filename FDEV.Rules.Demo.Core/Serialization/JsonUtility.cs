using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FDEV.Rules.Demo.Core.Serialization
{
    /// <summary>
    /// A JSON addition to my ISerializers
    /// </summary>
    public static class JsonUtility
    {
         // Returns JSON string
        public static string Get(string url, string oktaToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = @"application/json";
            request.Accept = @"application/json";

            if (!string.IsNullOrEmpty(oktaToken))
            {
                request.Headers.Add("Authorization", "SSWS " + oktaToken);
            }

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "Unhandled Error";
            }
        }

        // POST a JSON string
        public static string Post(string url, string jsonContent = null, string oktaToken = null, string oktaOAuthHeader = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = @"application/json";
            request.Accept = @"application/json";

            if (!string.IsNullOrEmpty(oktaToken))
            {
                request.Headers.Add("Authorization", "SSWS " + oktaToken);
            }

            if (!string.IsNullOrEmpty(oktaOAuthHeader))
            {
                request.Headers.Add("Authorization", "Basic " + oktaOAuthHeader);
            }

            if (!string.IsNullOrEmpty(jsonContent))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(jsonContent);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            else
            {
                request.ContentType = @"application/x-www-form-urlencoded";
            }

            try
            {
                var response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "Unhandled Error";
            }
        }

        public static string Delete(string url, string oktaToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = @"application/json";
            request.Accept = @"application/json";

            if (!string.IsNullOrEmpty(oktaToken))
            {
                request.Headers.Add("Authorization", "SSWS " + oktaToken);
            }

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "Unhandled Error";
            }
        }

        public static string JsonContent(object model)
        {
            if (model == null) return null;
            string json;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(model.GetType());
                serializer.WriteObject(ms, model);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                json = sr.ReadToEnd();
            }
            return json;
        }

        //private static async Task<List<Model>> DeserializeOptimizedFromStreamCallAsync(CancellationToken cancellationToken)
        //{
        //    using (var client = new HttpClient())
        //    using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
        //    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        //    {
        //        var stream = await response.Content.ReadAsStreamAsync();

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return DeserializeJsonFromStream<List<Model>>(stream);
        //        }

        //        var content = await StreamToStringAsync(stream);
        //        throw new ApiException
        //        {
        //            StatusCode = (int)response.StatusCode,
        //            Content = content
        //        };
        //    }
        //}

        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead) return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                return js.Deserialize<T>(jtr);
            }
        }

        public static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            using (var sr = new StreamReader(stream))
            {
                content = await sr.ReadToEndAsync();
            }
            return content;
        }
    }
}

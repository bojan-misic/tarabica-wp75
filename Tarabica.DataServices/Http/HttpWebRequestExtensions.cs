using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Tarabica.DataServices.Http
{
    public static class HttpWebRequestExtensions
    {
        public static IObservable<KeyValuePair<Uri, byte[]>> GetBytes(this HttpWebRequest request)
        {
            request.Method = "GET";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                        {
                            using (var rdr = new BinaryReader(response.GetResponseStream()))
                            {
                                using (var ms = new MemoryStream())
                                {
                                    var buffer = new byte[4096];
                                    int bytesRead = 0;
                                    while ((bytesRead = rdr.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        ms.Write(buffer, 0, bytesRead);
                                    }
                                    return new KeyValuePair<Uri, byte[]>(request.RequestUri, ms.ToArray());
                                }
                            }
                        });
        }

        public static IObservable<string> GetText(this HttpWebRequest request)
        {
            request.Method = "GET";
            request.Accept = "text/plain";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                        {
                            using (var rdr = new StreamReader(response.GetResponseStream()))
                            {
                                return rdr.ReadToEnd();
                            }
                        });
        }

        public static IObservable<T> GetJson<T>(this HttpWebRequest request)
        {
            request.Method = "GET";
            request.Accept = "application/json";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                        {
                            using (var rdr = new StreamReader(response.GetResponseStream()))
                            {
                                var result = rdr.ReadToEnd();
                                return JsonConvert.DeserializeObject<T>(result);
                            }
                        });
        }

        //public static IObservable<XDocument> GetXml(this HttpWebRequest request)
        //{
        //    request.Method = "GET";
        //    request.Accept = "application/json";

        //    return
        //        Observable
        //            .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
        //            .Select(
        //                response =>
        //                    {
        //                        return XDocument.Load(response.GetResponseStream());
        //                    });
        //}

        public static IObservable<Unit> PostJson<T>(this HttpWebRequest request, T obj)
        {

            request.Method = "POST";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (var sw = new StreamWriter(requestStream))
                            {
                                sw.WriteLine(JsonConvert.SerializeObject(obj));
                            }

                            return
                                Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)();
                        },
                        (requestStream, webResponse) => new Unit());
        }

        public static IObservable<R> PostJsonAndGetResult<T, R>(this HttpWebRequest request, T obj)
            where R : new()
        {
            request.Method = "POST";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (var sw = new StreamWriter(requestStream))
                            {
                                sw.WriteLine(JsonConvert.SerializeObject(obj));
                            }

                            return
                                Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)();
                        },
                        (requestStream, response) =>
                        {
                            using (var rdr = new StreamReader(response.GetResponseStream()))
                            {
                                var result = rdr.ReadToEnd();
                                if (string.IsNullOrEmpty(result)) return new R();
                                return JsonConvert.DeserializeObject<R>(result);
                            }
                        });
        }
    }
}

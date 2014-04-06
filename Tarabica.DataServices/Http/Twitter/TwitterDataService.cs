using Newtonsoft.Json;
using SharpGIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using Tarabica.Model.Dto.Twitter;

namespace Tarabica.DataServices.Http.Twitter
{
    public class TwitterDataService : ITwitterDataService
    {
        private const string consumerKey = "YOUR TWITTER CONSUMER KEY";
        private const string consumerSecret = "YOUR TWITTER CONSUMER SECRET";
        private const string requestTokenUrl = "https://api.twitter.com/oauth2/token";
        private const string searchTweetsUrl = "https://api.twitter.com/1.1/search/tweets.json";

        private string bearerToken = null;

        public IObservable<AuthorizationResponseDto> Authorize()
        {
            string bearerTokenCredentials = string.Format("{0}:{1}", consumerKey, consumerSecret);
            string bearerTokenCredentialsEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(bearerTokenCredentials));

            var request = WebRequestCreator.GZip.Create(new Uri(requestTokenUrl));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers[HttpRequestHeader.Authorization] = "Basic " + bearerTokenCredentialsEncoded;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (var sw = new StreamWriter(requestStream))
                            {
                                sw.Write("grant_type=client_credentials");
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
                                if (string.IsNullOrEmpty(result)) return new AuthorizationResponseDto();
                                AuthorizationResponseDto authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseDto>(result);
                                bearerToken = authorizationResponse.Access_token;
                                return authorizationResponse;
                            }
                        });
        }

        //public IObservable<TwitterDataModelDto> GetRecentTweets(TweetType tweetType)
        //{
        //    if (bearerToken != null)
        //    {
        //        string q = (tweetType == TweetType.User ? "a=from:mssinergija" : (tweetType == TweetType.Hashtag ? "q=%23mssinergija" : "q=from:mssinergija OR %23mssinergija"));
        //        string count = "count=20";
        //        string result_type = "result_type=recent";
        //        string include_entities = "include_entities=true";
        //        string query = "?" + q + "&" + count + "&" + include_entities + "&" + result_type;

        //        var request = WebRequestCreator.GZip.Create(new Uri(searchTweetsUrl + query));

        //        request.Method = "GET";
        //        request.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerToken;
        //        request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

        //        return
        //            Observable
        //                .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
        //                .Select(
        //                    response =>
        //                    {
        //                        using (var rdr = new StreamReader(response.GetResponseStream()))
        //                        {
        //                            var result = rdr.ReadToEnd();
        //                            return JsonConvert.DeserializeObject<TwitterDataModelDto>(result);
        //                        }
        //                    });
        //    }

        //    return Observable.Return(new TwitterDataModelDto());
        //}

        public IObservable<TwitterDataModelDto> GetNewTweets(TweetType tweetType, Int64 sinceId)
        {
            if (bearerToken != null)
            {
                string q = (tweetType == TweetType.User ? "q=from:msforge" : (tweetType == TweetType.Hashtag ? "q=%23tarabica14" : "q=from:msforge OR %23tarabica14"));
                string count = "count=20";
                string result_type = "result_type=recent";
                string include_entities = "include_entities=true";
                string since_id = String.Format("since_id={0}", sinceId);

                string query = "?" + q + "&" + count + "&" + include_entities + "&" + result_type + "&" + since_id;

                var request = WebRequestCreator.GZip.Create(new Uri(searchTweetsUrl + query));

                request.Method = "GET";
                request.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerToken;
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

                return
                    Observable
                        .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                        .Select(
                            response =>
                            {
                                using (var rdr = new StreamReader(response.GetResponseStream()))
                                {
                                    var result = rdr.ReadToEnd();
                                    return JsonConvert.DeserializeObject<TwitterDataModelDto>(result);
                                }
                            });
            }

            return Observable.Return(new TwitterDataModelDto());
        }

        public IObservable<TwitterDataModelDto> GetOlderTweets(TweetType tweetType, Int64 maxId)
        {
            if (bearerToken != null)
            {
                string q = (tweetType == TweetType.User ? "q=from:msforge" : (tweetType == TweetType.Hashtag ? "q=%23tarabica14" : "q=from:msforge OR %23tarabica14"));
                string count = "count=20";
                string result_type = "result_type=recent";
                string include_entities = "include_entities=true";
                string max_id = String.Format("max_id={0}", maxId);

                string query = "?" + q + "&" + count + "&" + include_entities + "&" + result_type + "&" + max_id;

                var request = WebRequestCreator.GZip.Create(new Uri(searchTweetsUrl + query));

                request.Method = "GET";
                request.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerToken;
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

                return
                    Observable
                        .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                        .Select(
                            response =>
                            {
                                using (var rdr = new StreamReader(response.GetResponseStream()))
                                {
                                    var result = rdr.ReadToEnd();
                                    return JsonConvert.DeserializeObject<TwitterDataModelDto>(result);
                                }
                            });
            }

            return Observable.Return(new TwitterDataModelDto());
        }
    }
}

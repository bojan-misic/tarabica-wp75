using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using Tarabica.DataServices.Http.Twitter;
using Tarabica.DataServices.Tasks;
using Tarabica.Model.Domain.Twitter;
using Tarabica.Model.Dto.Twitter;

namespace Tarabica.DataServices.Store.Twitter
{
    public class TwitterDataStore : Store, ITwitterDataStore
    {
        private const string twitterStoreName = "TwitterStore";
        private const string authorizingTwitterTaskName = "AuthorizingTwitterTask";
        private const string refreshingTweetsTaskName = "RefreshingTweetsTask";

        private TwitterDataModel twitterDataModel;
        private ITwitterDataService twitterService;

        public TwitterDataStore(ITwitterDataService twitterService)
        {
            this.twitterService = twitterService;
            twitterDataModel = TwitterDataModel.FromDto(GetDataFromJson<TwitterDataModelDto>(twitterStoreName));
        }

        public IEnumerable<Tweet> GetTweets()
        {
            return twitterDataModel.Tweets;
        }

        public IObservable<TaskCompletedSummary> Authorize()
        {
            return twitterService.Authorize().Select(
                            authorizationResponseDto =>
                            {
                                if (authorizationResponseDto.Token_type.Equals("bearer"))
                                {
                                    return new TaskCompletedSummary
                                    {
                                        Task = authorizingTwitterTaskName,
                                        Result = TaskSummaryResult.TwitterAuthorizationSuccess,
                                        Context = authorizationResponseDto.Access_token
                                    };
                                }
                                else
                                {
                                    return new TaskCompletedSummary
                                    {
                                        Task = authorizingTwitterTaskName,
                                        Result = TaskSummaryResult.TwitterAuthorizationFailure,
                                        Context = authorizationResponseDto.ToString()
                                    };
                                }
                            }).Catch(
                                    (Exception exception) =>
                                    {
                                        if (exception is WebException)
                                        {
                                            var webException = exception as WebException;
                                            var summary = ExceptionHandling.GetSummaryFromWebException(authorizingTwitterTaskName, webException);
                                            return Observable.Return(summary);
                                        }

                                        if (exception is UnauthorizedAccessException)
                                        {
                                            return Observable.Return(new TaskCompletedSummary { Task = authorizingTwitterTaskName, Result = TaskSummaryResult.AccessDenied });
                                        }

                                        ;
                                        return Observable.Return(
                                            new TaskCompletedSummary { Task = authorizingTwitterTaskName, Result = TaskSummaryResult.UnknownError }
                                            );
                                    });
        }

        //public IObservable<TaskCompletedSummary> GetRecentTweets(TweetType tweetType)
        //{
        //    return twitterService.GetRecentTweets(tweetType).Select(
        //         twitterDataModelDto =>
        //         {
        //             this.twitterDataModel = TwitterDataModel.FromDto(twitterDataModelDto);
        //             StoreDataToJson(twitterStoreName, twitterDataModelDto);

        //             return new TaskCompletedSummary
        //             {
        //                 Task = refreshingTweetsTaskName,
        //                 Result = TaskSummaryResult.Success,
        //                 Context = twitterDataModelDto.Statuses != null ? twitterDataModelDto.Statuses.Count.ToString() : "0"
        //             };
        //         }).Catch(
        //                 (Exception exception) =>
        //                 {
        //                     if (exception is WebException)
        //                     {
        //                         var webException = exception as WebException;
        //                         var summary = ExceptionHandling.GetSummaryFromWebException(refreshingTweetsTaskName, webException);
        //                         return Observable.Return(summary);
        //                     }

        //                     if (exception is UnauthorizedAccessException)
        //                     {
        //                         return Observable.Return(new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.AccessDenied });
        //                     }

        //                     ;
        //                     return Observable.Return(
        //                         new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.UnknownError }
        //                         );
        //                 });
        //}

        public IObservable<TaskCompletedSummary> GetNewTweets(TweetType tweetType)
        {
            return twitterService.GetNewTweets(tweetType, twitterDataModel.MaxId).Select(
                 twitterDataModelDto =>
                 {
                     TwitterDataModel.AddFromDto(twitterDataModelDto, this.twitterDataModel);
                     StoreDataToJson(twitterStoreName, this.twitterDataModel.DtoModel);

                     return new TaskCompletedSummary
                     {
                         Task = refreshingTweetsTaskName,
                         Result = TaskSummaryResult.Success,
                         Context = twitterDataModelDto.Statuses != null ? twitterDataModelDto.Statuses.Count.ToString() : "0"
                     };
                 }).Catch(
                         (Exception exception) =>
                         {
                             if (exception is WebException)
                             {
                                 var webException = exception as WebException;
                                 var summary = ExceptionHandling.GetSummaryFromWebException(refreshingTweetsTaskName, webException);
                                 return Observable.Return(summary);
                             }

                             if (exception is UnauthorizedAccessException)
                             {
                                 return Observable.Return(new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.AccessDenied });
                             }

                             ;
                             return Observable.Return(
                                 new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.UnknownError }
                                 );
                         });
        }

        public IObservable<TaskCompletedSummary> GetOlderTweets(TweetType tweetType)
        {
            return twitterService.GetOlderTweets(tweetType, twitterDataModel.MinId - 1).Select(
                 twitterDataModelDto =>
                 {
                     TwitterDataModel.AddFromDto(twitterDataModelDto, this.twitterDataModel);

                     return new TaskCompletedSummary
                     {
                         Task = refreshingTweetsTaskName,
                         Result = TaskSummaryResult.Success,
                         Context = twitterDataModelDto.Statuses != null ? twitterDataModelDto.Statuses.Count.ToString() : "0"
                     };
                 }).Catch(
                         (Exception exception) =>
                         {
                             if (exception is WebException)
                             {
                                 var webException = exception as WebException;
                                 var summary = ExceptionHandling.GetSummaryFromWebException(refreshingTweetsTaskName, webException);
                                 return Observable.Return(summary);
                             }

                             if (exception is UnauthorizedAccessException)
                             {
                                 return Observable.Return(new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.AccessDenied });
                             }

                             ;
                             return Observable.Return(
                                 new TaskCompletedSummary { Task = refreshingTweetsTaskName, Result = TaskSummaryResult.UnknownError }
                                 );
                         });
        }

        public void ClearStore()
        {
            DeleteStore(twitterStoreName);
            twitterDataModel = new TwitterDataModel();
        }
    }
}

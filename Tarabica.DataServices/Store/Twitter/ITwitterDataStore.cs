using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.DataServices.Http.Twitter;
using Tarabica.DataServices.Tasks;
using Tarabica.Model.Domain.Twitter;

namespace Tarabica.DataServices.Store.Twitter
{
    public interface ITwitterDataStore
    {
        IEnumerable<Tweet> GetTweets();

        IObservable<TaskCompletedSummary> Authorize();
        //IObservable<TaskCompletedSummary> GetRecentTweets(TweetType tweetType);
        IObservable<TaskCompletedSummary> GetNewTweets(TweetType tweetType);
        IObservable<TaskCompletedSummary> GetOlderTweets(TweetType tweetType);
        void ClearStore();
    }
}

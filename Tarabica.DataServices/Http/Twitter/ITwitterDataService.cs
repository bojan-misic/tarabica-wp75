using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.Model.Dto.Twitter;

namespace Tarabica.DataServices.Http.Twitter
{
    public interface ITwitterDataService
    {
        IObservable<AuthorizationResponseDto> Authorize();
        IObservable<TwitterDataModelDto> GetNewTweets(TweetType tweetType, Int64 sinceId);
        IObservable<TwitterDataModelDto> GetOlderTweets(TweetType tweetType, Int64 maxId);
    }
}

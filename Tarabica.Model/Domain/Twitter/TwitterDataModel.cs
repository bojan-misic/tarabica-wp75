using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tarabica.Model.Dto.Twitter;

namespace Tarabica.Model.Domain.Twitter
{
    public class TwitterDataModel
    {
        private Dictionary<Int64, Tweet> tweets;
        private TwitterDataModelDto dto;

        public TwitterDataModelDto DtoModel { get { return dto; } } // 20 newest

        public List<Tweet> Tweets
        {
            get
            {
                if (IsInitialized)
                    return tweets.Values.OrderByDescending(t => t.Id).ToList();
                else
                    return new List<Tweet>();
            }
        }

        public Int64 MinId
        {
            get
            {
                if (IsInitialized)
                    return tweets.Keys.Min();
                else
                    return -1;
            }
        }

        public Int64 MaxId
        {
            get
            {
                if (IsInitialized)
                    return tweets.Keys.Max();
                else
                    return -1;
            }
        }

        public bool IsInitialized
        {
            get { return tweets != null; }
        }

        public static TwitterDataModel FromDto(TwitterDataModelDto dtoModel)
        {
            var model = new TwitterDataModel();
            model.dto = dtoModel;

            // if dtoModel is not initialized return an empty model
            if (dtoModel == null || dtoModel.Statuses == null) return model;

            AddFromDto(dtoModel, model);
            return model;
        }

        public static void AddFromDto(TwitterDataModelDto dtoModel, TwitterDataModel model)
        {
            bool dtoExisted = true;

            if (!model.IsInitialized && dtoModel.Statuses.Count > 0)
            {
                model.tweets = new Dictionary<Int64, Tweet>();
                model.dto = dtoModel;
                dtoExisted = false;
            }

            foreach (var tweetDto in dtoModel.Statuses.OrderByDescending(t => t.Id))
            {
                List<string> urls = new List<string>();

                if (tweetDto.Entities != null)
                    foreach (var urlDto in tweetDto.Entities.Urls)
                        if (!String.IsNullOrEmpty(urlDto.Url))
                            urls.Add(urlDto.Url);

                model.tweets[tweetDto.Id] = new Tweet()
                {
                    Id = tweetDto.Id,
                    Text = tweetDto.Text,
                    Time = DateTime.ParseExact(tweetDto.Created_at, "ddd MMM dd HH:mm:ss %K yyyy", CultureInfo.InvariantCulture.DateTimeFormat),
                    Since = LocalizedDateHelper.GetPrettyDateSince(DateTime.ParseExact(tweetDto.Created_at, "ddd MMM dd HH:mm:ss %K yyyy", CultureInfo.InvariantCulture.DateTimeFormat)),
                    Urls = urls,
                    User = new User() { Name = tweetDto.User.Name, ScreenName = "@" + tweetDto.User.Screen_name.Substring(0, Math.Min(tweetDto.User.Screen_name.Length, 12)), ImageUrl = tweetDto.User.Profile_image_url_https }
                };

                if (dtoExisted)
                    model.dto.Statuses.Add(tweetDto);
            }
            
            if (model.dto.Statuses != null)
                model.dto.Statuses = model.dto.Statuses.OrderByDescending(t => t.Id).Distinct().Take(20).ToList();
        }
    }
}

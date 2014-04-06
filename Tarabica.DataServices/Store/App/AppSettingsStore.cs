//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using Tarabica.DataServices.Http.Twitter;

namespace Tarabica.DataServices.Store.App
{
    /// <summary>
    /// Persistent Application Settings Provider.
    /// </summary>
    public class AppSettingsStore : IAppSettingsStore
    {
        #region Private fields
        private const TweetType tweetTypeDefaultValue = TweetType.Hashtag;
        private const string tweetTypeSettingKey = "TweetType";

        private readonly IsolatedStorageSettings isolatedStorage;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsStore"/> class.
        /// </summary>
        public AppSettingsStore()
        {
            this.isolatedStorage = IsolatedStorageSettings.ApplicationSettings;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating what type of tweets user wants to receive.
        /// </summary>
        /// <value>
        /// <c>User</c> if user wants to receive @mssinergija tweets;
        /// <c>Hashtag</c> if user wants to receive #mssinergija tweets;
        /// <c>UserAndHashtag</c> if user wants to receive both type of tweets;
        /// </value>
        public TweetType TweetType
        {
            get { return this.GetValueOrDefault(tweetTypeSettingKey, tweetTypeDefaultValue); }
            set { this.AddOrUpdateValue(tweetTypeSettingKey, value); }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Adds or updates the value for the setting.
        /// </summary>
        /// <param name="key">The key for the setting.</param>
        /// <param name="value">The value of the setting.</param>
        private void AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;

            try
            {
                // if new value is different, set the new value.
                if (this.isolatedStorage[key] != value)
                {
                    this.isolatedStorage[key] = value;
                    valueChanged = true;
                }
            }
            catch (KeyNotFoundException)
            {
                this.isolatedStorage.Add(key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                this.isolatedStorage.Add(key, value);
                valueChanged = true;
            }

            if (valueChanged)
            {
                this.isolatedStorage.Save();
            }
        }

        /// <summary>
        /// Gets the (modified or default) value for the setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key for the setting.</param>
        /// <param name="defaultValue">The default value for the setting.</param>
        /// <returns></returns>
        private T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            try
            {
                value = (T)this.isolatedStorage[key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            }

            return value;
        }
        #endregion
    }
}

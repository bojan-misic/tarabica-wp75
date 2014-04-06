using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.Model.Domain.Twitter;

namespace Tarabica.Model.Domain
{
    public class TweetEqualityComparer : IEqualityComparer<Tweet>
    {
        public bool Equals(Tweet x, Tweet y)
        {
            if (x.Id == y.Id)
                return true;
            else
                return false;
        }

        public int GetHashCode(Tweet obj)
        {
            // Is this ok?
            return obj.Id.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Tarabica.Model
{
    public class ColorConverter
    {
        public static Color GetColorFromString(string sColor)
        {
            var s = sColor.Trim().TrimStart('#');

            // only 8 (with alpha channel) or 6 symbols are allowed
            //if (s.Length != 8 && s.Length != 6)
            //    throw new ArgumentException("Unknown string format!");

            int startParseIndex = 0;
            bool alphaChannelExists = s.Length == 8; // check if alpha channel exists            

            // read alpha channel value
            byte a = 255;
            if (alphaChannelExists)
            {
                a = System.Convert.ToByte(s.Substring(0, 2), 16);
                startParseIndex += 2;
            }

            // read r value
            byte r = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read g value
            byte g = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read b value
            byte b = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            var color = Color.FromArgb(a, r, g, b);
            return color;
        }
    }
}

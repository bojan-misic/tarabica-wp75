using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tarabica.Model.Domain.Conference;
using Tarabica.WP7App.Services;
using Microsoft.Practices.Prism.Commands;

namespace Tarabica.WP7App.ViewModels
{
    public class SessionInfoViewModel : ViewModel
    {
        private readonly Session session;

        public SessionInfoViewModel(
            Session session,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.session = session;
        }

        public Session Session
        {
            get { return session; }
        }

        public int Id { get { return session.Id; } }
        public string Title { get { return session.Title; } }
        public string Slot { get { return session.Slot.ToTimeSpanString(); } }
        public int Level { get { return session.Level; } }
        public string Language { get { return session.LanguageCode; } }
        public string Room { get { return session.Room.Code; } }
        public string TrackCode { get { return session.Track.Abbeveration; } }
        public string TrackTitle { get { return "[" + session.Track.Title + "]"; } }
        public string MinorInfo { get { return string.Format("{0} {1} {2} {3}", TrackCode, Level, Room, Language); } }
        public string FullInfo { get { return string.Format("{0}, {1} {2} {3} {4}", SlotInfo, TrackCode, Level, Room, Language); } }

        public bool IsFavourite { get { return session.IsFavourite; } }

        public string TimeInfo
        {
            get { return session.Slot.ToTimeSpanString(); }
        }


        public string SlotInfo
        {
            get { return string.Format("{0} {1}", session.Slot.Day.DayName, session.Slot.ToTimeSpanString()); }
        }

        public Brush TrackColor
        {
            get
            {
                //return new SolidColorBrush(
                //HandleWhiteColor(session.Track.Color));
                return new SolidColorBrush(Color.FromArgb(0xff,0x2e,0x99,0xcf));
            }
        }
        //dd2e99cf
        public Brush TrackBackgroundColor
        {
            get
            {
                //return new SolidColorBrush(
                //HandleWhiteColor(session.Track.BackgroundColor));
                return new SolidColorBrush(Color.FromArgb(0xdd, 0x2e, 0x99, 0xcf));
            }
        }

        //private Color HandleWhiteColor(Color color)
        //{
        //    if (LightThemeEnabled)
        //    {
        //        if (color == Colors.White)
        //        {
        //            return Colors.Black;
        //        }
        //    }
        //    else
        //    {
        //        if (color == Colors.Black)
        //        {
        //            return Colors.White;
        //        }
        //    }
        //    return color;
        //}

        public bool LightThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        public string Speakers
        {
            get
            {
                return session.Speakers
                    .Select(s => s.FullName)
                    .Aggregate("", (acc, name) => acc + "\n" + name);
            }
        }

        public bool TrackHasThreeChars
        {
            get { return TrackCode.Length == 3; }
        }

        public override void IsBeingActivated()
        {}
    }
}

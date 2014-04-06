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
using Tarabica.DataServices.Store.Conference;
using Tarabica.WP7App.Services;
using Tarabica.Model.Domain.Conference;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Tarabica.WP7App.Infrastructure;
using System.Text.RegularExpressions;

namespace Tarabica.WP7App.ViewModels
{
    public class SessionDetailsViewModel : ViewModel
    {
        private readonly IConferenceDataStoreLocator conferenceDataStoreLocator;

        private ObservableCollection<SpeakerInfoViewModel> sessionSpeakersObservableCollection;
        private CollectionViewSource sessionSpeakersViewSource;
        private int fontSize = 32;

        public SessionDetailsViewModel(
            IConferenceDataStoreLocator conferenceDataStoreLocator,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.conferenceDataStoreLocator = conferenceDataStoreLocator;
            initialize();
        }

        private void initialize()
        {
            this.SpeakerSelectedCommand = new DelegateCommand<SpeakerInfoViewModel>(
                s =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SpeakerDetailsView.xaml?Id=" + s.Id, UriKind.Relative));
                });

            this.ToggleFavCommand = new DelegateCommand(
                () =>
                {
                    IsFavourite = !IsFavourite;
                });

            this.FloormapViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/FloormapView.xaml?Id=" + Session.Room.Id, UriKind.Relative));
                });

            this.PageLoadedCommand = new DelegateCommand(
            () =>
            {
                session = conferenceDataStoreLocator.GetStore().FindSession(id.Value);
                this.RaisePropertyChanged(() => this.AppBarFavIconUri);
                this.RaisePropertyChanged(() => this.AppBarFavText);
            });

            //this.MapCommand = new DelegateCommand(
            //    () =>
            //    {
            //        var room = Session.Room.Code;
            //        if (room.EndsWith("/1") || room.EndsWith("salon"))
            //        {
            //            NavigationService.Navigate(
            //                new Uri("/Views/Map1View.xaml?ID=" + room, UriKind.Relative));
            //        }
            //        else
            //        {
            //            NavigationService.Navigate(
            //                new Uri("/Views/Map0View.xaml?ID=" + room, UriKind.Relative));
            //        }
            //    });

            this.IsBeingActivated();
        }

        private int? id;
        private Session session;
        public Session Session
        {
            get
            {
                if (this.id == null)
                {
                    id = int.Parse(NavigationService.CurrentSource.OriginalString.Split('=')[1]);
                    session = conferenceDataStoreLocator.GetStore().FindSession(id.Value);
                    //RaisePropertyChanged(() => Description);
                }
                return session;
            }
        }
  
        public string Track { get { return Session.Track.Abbeveration; } }
        public string Slot { get { return Session.Slot.ToTimeSpanString(); } }

        public int Level { get { return Session.Level; } }
        public string Language { get { return Session.LanguageCode; } }
        public string Room { get { return Session.Room.Code; } }
        public string TrackCode { get { return Session.Track.Abbeveration; } }
        public string MinorInfo { get { return string.Format("{0} {1} {2} {3}", TrackCode, Level, Room, Language); } }
        public string FullInfo { get { return string.Format("{0}, {1} {2} {3} {4}", SlotInfo, TrackCode, Level, Room, Language); } }

        public string Title 
        { 
            get 
            {
                var charCount = Session.Title.Length;

                if (charCount > 90)
                    FontSize = 20;
                else if (charCount > 70)
                    FontSize = 24;
                else if (charCount > 50)
                    FontSize = 28;

                return Session.Title; 
            } 
        }

        public string TrackTitle
        {
            get
            {
                return "[" + Session.Track.Title + "]";
            }
        }

        public Brush TrackColor
        {
            get
            {
                //return new SolidColorBrush(
                //HandleWhiteColor(session.Track.Color));
                return new SolidColorBrush(Color.FromArgb(0xff, 0x2e, 0x99, 0xcf));
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

        private Color HandleWhiteColor(Color color)
        {
            if (LightThemeEnabled)
            {
                if (color == Colors.White)
                {
                    return Colors.Black;
                }
            }
            else
            {
                if (color == Colors.Black)
                {
                    return Colors.White;
                }
            }
            return color;
        }

        public bool LightThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        public string SlotInfo
        {
            get { return Session.Slot.ToTimeSpanString(); }
        }

        public string ShortSlotInfo
        {
            get
            {
                return 
                Session.Slot.ToTimeSpanString(); }
        }

        public int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                this.RaisePropertyChanged(() => FontSize);
            }
        }

        public bool IsFavourite
        {
            get
            {
                return Session.IsFavourite;
            }
            set
            {
                Session.IsFavourite = value;
                this.RaisePropertyChanged(() => this.AppBarFavIconUri);
                this.RaisePropertyChanged(() => this.AppBarFavText);
                conferenceDataStoreLocator.GetStore().SaveFavouritesStore();
            }
        }

        public Uri AppBarFavIconUri
        {
            get
            {
                return new Uri(IsFavourite ?
                    (ThemeLocator.LightThemeEnabled ? "/Assets/Light/appbar.full.heart.png" : "/Assets/Dark/appbar.full.heart.png") :
                    (ThemeLocator.LightThemeEnabled ? "/Assets/Light/appbar.semi.heart.png" : "/Assets/Dark/appbar.semi.heart.png"), 
                    UriKind.Relative);
            }
        }

        public bool TrackHasThreeChars
        {
            get { return TrackCode.Length == 3; }
        }

        //public Uri AppBarShowOnMapIconUri
        //{
        //    get
        //    {
        //        return new Uri(ThemeLocator.LightThemeEnabled ?
        //            "/Resources/Icons/Light/appbar.nav.png" :
        //            "/Resources/Icons/Dark/appbar.nav.png", UriKind.Relative);
        //    }
        //}

        //public string AppBarShowOnMapText
        //{
        //    get
        //    {
        //        return "na mapi";
        //    }
        //}

        public string AppBarFavText
        {
            get
            {
                return IsFavourite ?
                    "ukloni iz odabranog" :
                    "odaberi";
            }
        }

        public string Description
        {
            get
            {
                return session.Description.Replace("\r\n", String.Empty);
            }
        }

        public ICollectionView Speakers
        {
            get
            {
                sessionSpeakersObservableCollection = new ObservableCollection<SpeakerInfoViewModel>();
                Session.Speakers.Select(s => new SpeakerInfoViewModel(s, NavigationService)).ToList()
                    .ForEach(sessionSpeakersObservableCollection.Add);

                sessionSpeakersViewSource = new CollectionViewSource { Source = sessionSpeakersObservableCollection };

                return sessionSpeakersViewSource.View;
            }
        }

        public DelegateCommand<SpeakerInfoViewModel> SpeakerSelectedCommand { get; set; }
        public DelegateCommand ToggleFavCommand { get; set; }
        public DelegateCommand FloormapViewCommand { get; set; }
        public DelegateCommand PageLoadedCommand { get; set; }

        public override void IsBeingActivated()
        {}
    }
}

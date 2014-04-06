using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tarabica.DataServices.Http.Conference;
using Tarabica.Model.Domain.Conference;
using Tarabica.DataServices.Store.Conference;
using Tarabica.WP7App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tarabica.WP7App.Infrastructure;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Navigation;
using System.Net;
using System.Reactive;

namespace Tarabica.WP7App.ViewModels
{
    public class SessionListViewModel : ViewModel
    {
        //private readonly IConfDataStoreLocator confDataStoreLocator; //ctor param

        private readonly IConferenceDataStore conferenceDataStore;

        private ObservableCollection<GroupList<SessionInfoViewModel>> sessions0ObservableCollection;
        private CollectionViewSource sessions0ViewSource;

        private bool isSessions0Empty;

        public SessionListViewModel(
            IConferenceDataStoreLocator conferenceDataStoreLocator,
            INavigationService navigationService)
            : base(navigationService)
        {
            //this.confDataStoreLocator = confDataStoreLocator;
            this.conferenceDataStore = conferenceDataStoreLocator.GetStore();
            initialize();
        }

        private void initialize()
        {
            sessions0ViewSource = new CollectionViewSource();

            this.SessionSelectedCommand = new DelegateCommand<SessionInfoViewModel>(
                s =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SessionDetailsView.xaml?Id=" + s.Id, UriKind.Relative));
                });

            this.ToggleFavCommand0 = new DelegateCommand<SessionInfoViewModel>(
                s =>
                {
                    s.Session.IsFavourite = !s.IsFavourite;
                    conferenceDataStore.SaveFavouritesStore();
                    refreshSessions0(IsFavouriteMode, Track.Id);
                });

            //this.ToggleFavCommand1 = new DelegateCommand<SessionInfoViewModel>(
            //    s =>
            //    {
            //        s.Session.IsFavourite = !s.IsFavourite;
            //        conferenceDataStore.SaveFavouritesStore();
            //        refreshSessions1(IsFavouriteMode, Track.Id);
            //    });

            this.SelectionChangedCommand = new DelegateCommand(
                () =>
                {
                    CurrentPivotItemIndex = Math.Abs(CurrentPivotItemIndex - 1);
                });

            this.IsBeingActivated();
        }

        public DelegateCommand<SessionInfoViewModel> SessionSelectedCommand { get; set; }
        public DelegateCommand<SessionInfoViewModel> ToggleFavCommand0 { get; set; }
        //public DelegateCommand<SessionInfoViewModel> ToggleFavCommand1 { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }
        public DelegateCommand PageLoadedCommand { get; set; }

        private int? id;
        private Speaker speaker;
        public Speaker Speaker
        {
            get
            {           
                if (this.id == null)
                {
                    var id = int.Parse(NavigationService.CurrentSource.OriginalString.Split('=')[1]);
                    this.id = id;
                    speaker = conferenceDataStore.FindSpeaker(id);
                }
                return speaker;
            }
        }

        private int? trackId;
        private Track track;
        public Track Track
        {
            get
            {    
                if (this.trackId == null)
                {
                    var id = int.Parse(NavigationService.CurrentSource.OriginalString.Split('&')[1].Split('=')[1]);
                    this.trackId = id;
                    this.track = conferenceDataStore.FindTrack(id);
                }
                return this.track;
            }
        }

        private int? isFavouriteModeInt;
        private bool isFavouriteMode;
        public bool IsFavouriteMode
        {
            get
            {            
                if (this.isFavouriteModeInt == null)
                {
                    var boolInt = int.Parse(NavigationService.CurrentSource.OriginalString.Split('&')[2].Split('=')[1]);
                    this.isFavouriteModeInt = boolInt;
                    this.isFavouriteMode = boolInt == 0 ? false : true;
                }
                return this.isFavouriteMode;
            }
        }

        private int? currentPivotItemIndexId;
        private int currentPivotItemIndex;
        public int CurrentPivotItemIndex
        {
            get
            {         
                if (this.currentPivotItemIndexId == null)
                {
                    var indexId = int.Parse(NavigationService.CurrentSource.OriginalString.Split('&')[0].Split('=')[1]);
                    this.currentPivotItemIndexId = indexId;
                    this.currentPivotItemIndex = indexId;
                }
                return this.currentPivotItemIndex;
            }
            set
            {
                this.currentPivotItemIndex = value;
                this.RaisePropertyChanged(() => this.CurrentPivotItemIndex);
            }
        }

        public ICollectionView Sessions0
        {
            get
            {
                return sessions0ViewSource.View;
            }
        }

        private void refreshSessions0(bool isFavouriteMode, int trackId)
        {
            int dayId = conferenceDataStore.GetDays().Skip(1).First().Id;
            sessions0ObservableCollection = new ObservableCollection<GroupList<SessionInfoViewModel>>();

            var sessionGroups = isFavouriteMode ? // only favourites?
                (trackId == -1 ? // all tracks?
                groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.IsFavourite)) :
                groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId && s.IsFavourite)))
                :
                (trackId == -1 ? // all tracks?
                groupSessions(conferenceDataStore.GetSessionsByDay(dayId)) :
                groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId)));

            sessionGroups.ForEach(sessions0ObservableCollection.Add);

            sessions0ViewSource = new CollectionViewSource { Source = sessions0ObservableCollection };
            IsSessions0Empty = sessions0ObservableCollection.Count == 0 ? true : false;
            RaisePropertyChanged(() => this.Sessions0);
            //var o = Observable.Start(
            //    () =>
            //    {
            //        int dayId = confDataStore.GetDays().Where(d => d.Date.DayOfWeek == DayOfWeek.Tuesday).Select(d => d.Id).First();
            //        sessions0ObservableCollection = new ObservableCollection<GroupList<SessionInfoViewModel>>();

            //        var sessionGroups = isFavouriteMode ? // only favourites?
            //            (trackId == 0 ? // all tracks?
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.IsFavourite)) :
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId && s.IsFavourite)))
            //            :
            //            (trackId == 0 ? // all tracks?
            //            groupSessions(confDataStore.GetSessionsByDay(dayId)) :
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId)));

            //        sessionGroups.ForEach(sessions0ObservableCollection.Add);
                
            //    }).ObserveOnDispatcher().Subscribe(
            //    _ =>
            //    {
            //        sessions0ViewSource = new CollectionViewSource { Source = sessions0ObservableCollection };
            //        IsSessions0Empty = sessions0ObservableCollection.Count == 0 ? true : false;
            //        RaisePropertyChanged(() => this.Sessions0);
            //    });
        }

        //public ICollectionView Sessions1
        //{
        //    get
        //    {
        //        return sessions1ViewSource.View;
        //    }
        //}

        //private void refreshSessions1(bool isFavouriteMode, int trackId)
        //{
        //    int dayId = conferenceDataStore.GetDays().Where(d => d.Date.DayOfWeek == DayOfWeek.Wednesday).Select(d => d.Id).First();
        //    sessions1ObservableCollection = new ObservableCollection<GroupList<SessionInfoViewModel>>();

        //    var sessionGroups = isFavouriteMode ? // only favourites?
        //        (trackId == 0 ? // all tracks?
        //        groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.IsFavourite)) :
        //        groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId && s.IsFavourite)))
        //        :
        //        (trackId == 0 ? // all tracks?
        //        groupSessions(conferenceDataStore.GetSessionsByDay(dayId)) :
        //        groupSessions(conferenceDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId)));

        //    sessionGroups.ForEach(sessions1ObservableCollection.Add);

        //    sessions1ViewSource = new CollectionViewSource { Source = sessions1ObservableCollection };
        //    IsSessions1Empty = sessions1ObservableCollection.Count == 0 ? true : false;
        //    RaisePropertyChanged(() => this.Sessions1);
            
            //var o = Observable.Start(
            //    () =>
            //    {
            //        int dayId = confDataStore.GetDays().Where(d => d.Date.DayOfWeek == DayOfWeek.Wednesday).Select(d => d.Id).First();
            //        sessions1ObservableCollection = new ObservableCollection<GroupList<SessionInfoViewModel>>();

            //        var sessionGroups = isFavouriteMode ? // only favourites?
            //            (trackId == 0 ? // all tracks?
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.IsFavourite)) :
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId && s.IsFavourite)))
            //            :
            //            (trackId == 0 ? // all tracks?
            //            groupSessions(confDataStore.GetSessionsByDay(dayId)) :
            //            groupSessions(confDataStore.GetSessionsByDay(dayId).Where(s => s.Track.Id == trackId)));

            //        sessionGroups.ForEach(sessions1ObservableCollection.Add);

            //    }).ObserveOnDispatcher().Subscribe(
            //    _ =>
            //    {
            //        sessions1ViewSource = new CollectionViewSource { Source = sessions1ObservableCollection };
            //        IsSessions1Empty = sessions1ObservableCollection.Count == 0 ? true : false;
            //        RaisePropertyChanged(() => this.Sessions1);
            //    });
        //}

        private List<GroupList<SessionInfoViewModel>> groupSessions(IEnumerable<Session> sessions)
        {
            var result = new List<GroupList<SessionInfoViewModel>>();
            var slots = sessions.OrderBy(s => s.Slot.Day.Date).ThenBy(s => s.Slot.ToTimeSpanString()).Select(s => s.Slot.ToTimeSpanString()).Distinct();
            var sessionInfos = sessions.OrderBy(s => s.Slot.Day.Date).ThenBy(s => s.Slot.ToTimeSpanString()).Select(s => new SessionInfoViewModel(s, NavigationService));
            var groups = new Dictionary<string, GroupList<SessionInfoViewModel>>();
            foreach (var slot in slots)
            {
                var group = new GroupList<SessionInfoViewModel>(slot);
                result.Add(group);
                groups[slot] = group;
            }
            foreach (var sessionInfo in sessionInfos)
            {
                groups[sessionInfo.Slot.Trim()].Add(sessionInfo);
            }
            return result;
        }

        public bool IsSessions0Empty
        {
            get
            {
                return isSessions0Empty;
            }
            set
            {
                isSessions0Empty = value;
                RaisePropertyChanged(() => IsSessions0Empty);
            }
        }

        //public bool IsSessions1Empty
        //{
        //    get
        //    {
        //        return isSessions1Empty;
        //    }
        //    set
        //    {
        //        isSessions1Empty = value;
        //        RaisePropertyChanged(() => IsSessions1Empty);
        //    }
        //}

        public override void HandleOnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            refreshSessions0(IsFavouriteMode, Track.Id);
            //refreshSessions1(IsFavouriteMode, Track.Id);
        }

        public override void IsBeingActivated()
        {
        }

        public override void IsBeingDeactivated()
        {
        }
    }
}
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Tarabica.Model.Domain.Conference;
using Tarabica.Model.Domain;
using Tarabica.Model.Domain.Twitter;
using Tarabica.DataServices.Store.App;
using Tarabica.DataServices.Store.Conference;
using Tarabica.DataServices.Store.Twitter;
using Tarabica.DataServices.Tasks;
using Tarabica.WP7App.Infrastructure;
using Tarabica.WP7App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

using Notification = Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification;
using System.Reactive.Linq;
using System.Threading;

namespace Tarabica.WP7App.ViewModels
{
    public class MainViewModel : ViewModel
    {
        //private const int eventShuffleTime = 8; // 8 seconds

        private readonly IConferenceDataStoreLocator conferenceDataStoreLocator; //ctor param
        private readonly ITwitterDataStoreLocator twitterDataStoreLocator; //ctor param
        private readonly IAppSettingsStoreLocator appSettingsStoreLocator; //ctor param

        private readonly IConferenceDataStore conferenceDataStore;
        private readonly ITwitterDataStore twitterDataStore;
        private readonly IAppSettingsStore appSettingsStore;

        private ObservableCollection<Tweet> tweets;
        private ObservableCollection<Track> tracks;
        //private ObservableCollection<Tarabica.DataModel.Models.Conf.Event> currentEvent; // collection for only one element for
                                                                                            // insert animation behaviour
        //private List<Tarabica.DataModel.Models.Conf.Event> events;

        public DelegateCommand<Tweet> TweetSelectedCommand { get; set; }
        public DelegateCommand<Track> TrackSelectedCommand { get; set; }
        public DelegateCommand GetNewTweetsCommand { get; set; }
        public DelegateCommand ConfInfoViewCommand { get; set; }
        public DelegateCommand FavouritesViewCommand { get; set; }
        public DelegateCommand GetOlderTweetsCommand { get; set; }
        public DelegateCommand FirstDayViewCommand { get; set; }
        public DelegateCommand SpeakerListViewCommand { get; set; }

        private InteractionRequest<Notification> confErrorInteractionRequest;
        private InteractionRequest<Notification> twitterErrorInteractionRequest;
        private InteractionRequest<Notification> internetNotPresentInteractionRequest;

        private string minutesText;
        private string hoursText;
        private string daysText;

        //private int currentEventIndex;
        //private int eventTimerCount;
        private int minutesLeft;
        private int hoursLeft;
        private int daysLeft;

        private DispatcherTimer timer;
        private int selectedPivotIndex;
        private bool isSyncing;
        private bool isTweetsComing;
        private bool hasTweets = false;
        private bool isLoaded = false;

        private bool isTwitterInitiallyRefreshed = false;

        private DateTime startTime = new DateTime(2014, 4, 5, 8, 0, 0);

        public MainViewModel(
            IConferenceDataStoreLocator conferenceDataStoreLocator,
            ITwitterDataStoreLocator twitterDataStoreLocator,
            IAppSettingsStoreLocator appSettingsStoreLocator,
            INavigationService navigationService
            ): base(navigationService)
        {
            this.conferenceDataStoreLocator = conferenceDataStoreLocator;
            this.twitterDataStoreLocator = twitterDataStoreLocator;
            this.appSettingsStoreLocator = appSettingsStoreLocator;

            this.conferenceDataStore = conferenceDataStoreLocator.GetStore();
            this.twitterDataStore = twitterDataStoreLocator.GetStore();
            this.appSettingsStore = appSettingsStoreLocator.GetStore();

            this.tweets = new ObservableCollection<Tweet>();
            this.tracks = new ObservableCollection<Track>();
            //this.currentEvent = new ObservableCollection<DataModel.Models.Conf.Event>();

            //this.events = new List<DataModel.Models.Conf.Event>();

            this.confErrorInteractionRequest = new InteractionRequest<Notification>();
            this.twitterErrorInteractionRequest = new InteractionRequest<Notification>();
            this.internetNotPresentInteractionRequest = new InteractionRequest<Notification>();

            this.timer = new DispatcherTimer();

            initialize();
        }

        private void initialize()
        {
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Tick += new EventHandler(timer_Tick);

            this.ConfInfoViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/ConferenceInfoView.xaml", UriKind.Relative));
                });

            //this.EventInfoViewCommand = new DelegateCommand(
            //() =>
            //{
            //    NavigationService.Navigate(
            //        new Uri(String.Format("/Views/EventInfoView.xaml?eventId={0}", events[currentEventIndex].Id), UriKind.Relative));
            //});

            this.FirstDayViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SessionListView.xaml?dayId=0&trackId=-1&favs=0", UriKind.Relative));
                });
                //() => !this.IsSynchronizing);

            this.SpeakerListViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SpeakerListView.xaml", UriKind.Relative));
                });
                //() => !this.IsSynchronizing);

            this.FavouritesViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SessionListView.xaml?dayId=0&trackId=-1&favs=1", UriKind.Relative));
                });
                //() => !this.IsSynchronizing);

            this.TweetSelectedCommand = new DelegateCommand<Tweet>(
                t =>
                {
                    if (t == null) return;

                    if (t.Urls.Count > 0)
                    {
                        WebBrowserTask webBrowserTask = new WebBrowserTask();
                        webBrowserTask.Uri = new Uri(t.Urls[0]);
                        webBrowserTask.Show();
                    }
                });

            this.TrackSelectedCommand = new DelegateCommand<Track>(
                t =>
                {
                    if (t == null) return;
                    NavigationService.Navigate(
                        new Uri(String.Format("/Views/SessionListView.xaml?dayId=0&trackId={0}&favs=0", t.Id), UriKind.Relative));
                });

            this.GetNewTweetsCommand = new DelegateCommand(
                () =>
                {
                    getNewTweets();
                },
                () => !this.IsSynchronizing && !this.IsTweetsComing);

            this.GetOlderTweetsCommand = new DelegateCommand(
                () =>
                {
                    getOlderTweets();
                },
                () => !this.IsSynchronizing && !this.IsTweetsComing);

            loadTweets();
            loadConferenceData();

            if (!HasTarabicaStarted)
                this.timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var timeToSinergija = this.startTime - DateTime.Now;

            if (DateTime.Now > this.startTime)
                RaisePropertyChanged(() => HasTarabicaStarted);
            else
            {
                this.DaysLeft = timeToSinergija.Days;
                this.HoursLeft = timeToSinergija.Hours;
                this.MinutesLeft = timeToSinergija.Minutes;
            }
   
            //if (this.events.Count > 1)
            //{
            //    if (this.eventTimerCount == 0)
            //    {
            //        if (this.currentEvent.Count > 0)
            //            this.currentEvent.Clear();

            //        this.currentEventIndex = (this.currentEventIndex + 1) % this.events.Count;
            //        this.currentEvent.Add(this.events[this.currentEventIndex]);
            //    }

            //    this.eventTimerCount = (this.eventTimerCount + 1) % eventShuffleTime;
            //}          
        }

        // ASYNC
        private void refreshConferenceData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                IsSynchronizing = true;
                this.conferenceDataStore.CheckIfNewVersionIsAvailable().ObserveOn(SynchronizationContext.Current).Subscribe(
                    versionTaskCompletedSummary =>
                    {
                        if (versionTaskCompletedSummary.Result == TaskSummaryResult.NewVersionAvailable)
                            this.conferenceDataStore.RefreshConferenceData().ObserveOn(SynchronizationContext.Current).Subscribe(
                                refreshDataTaskCompletedSummary =>
                                {
                                    if (refreshDataTaskCompletedSummary.Result == TaskSummaryResult.Success)
                                        newConferenceDataArrived();
                                    else
                                        webServiceError();
                                });
                        else
                            alreadyHaveTheNewestVersion();
                    });
            }
            else
            {
                this.internetNotPresentInteractionRequest.Raise(
                    new Notification()
                    {
                        Title = "Tarabica",
                        Content = "Niste konektovani na Internet. Aplikacija će koristiti prethodno sačuvane podatke."
                    },
                    n => { });
            }
        }

        public override void HandleOnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!isLoaded)
            {
                refreshConferenceData();
                isLoaded = true;
            }

            RaisePropertyChanged(() => this.HasFavourites);

            // Tweets have been cleared from settings page:
            if (twitterDataStore.GetTweets().Count() == 0 && Tweets.Count > 0)
            {
                this.tweets = new ObservableCollection<Tweet>();
                RaisePropertyChanged(() => Tweets); // dodati u Tweets get set
                HasTweets = false;
            }
        }

        // ASYNC
        private void getNewTweets()
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var tweetType = appSettingsStoreLocator.GetStore().TweetType;
                IsSynchronizing = true;

                this.twitterDataStore.Authorize().ObserveOn(SynchronizationContext.Current).Subscribe(
                    twitterAuthorizationTaskSummary =>
                    {
                        if (twitterAuthorizationTaskSummary.Result == TaskSummaryResult.TwitterAuthorizationSuccess)
                            this.twitterDataStore.GetNewTweets(tweetType).ObserveOn(SynchronizationContext.Current).Subscribe(
                                refreshTweetsTaskSummary =>
                                {
                                    if (refreshTweetsTaskSummary.Result == TaskSummaryResult.Success)
                                    {
                                        if (!isTwitterInitiallyRefreshed)
                                            isTwitterInitiallyRefreshed = true;
                                        loadNewTweets();                                   
                                    }
                                    else
                                        refreshTwitterDataFailed(refreshTweetsTaskSummary);
                                });
                        else
                            refreshTwitterDataFailed(twitterAuthorizationTaskSummary);
                    });
            }
            else
            {
                this.internetNotPresentInteractionRequest.Raise(
                    new Notification()
                    {
                        Title = "Tarabica",
                        Content = "Niste konektovani na Internet."
                    },
                    n => { });
            }
        }

        // ASYNC
        private void getOlderTweets()
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var tweetType = appSettingsStoreLocator.GetStore().TweetType;
                IsSynchronizing = true;

                this.twitterDataStore.Authorize().ObserveOn(SynchronizationContext.Current).Subscribe(
                    twitterAuthorizationTaskSummary =>
                    {
                        if (twitterAuthorizationTaskSummary.Result == TaskSummaryResult.TwitterAuthorizationSuccess)
                            this.twitterDataStore.GetOlderTweets(tweetType).ObserveOn(SynchronizationContext.Current).Subscribe(
                                refreshTweetsTaskSummary =>
                                {
                                    if (refreshTweetsTaskSummary.Result == TaskSummaryResult.Success)
                                        loadOlderTweets();
                                    else
                                        refreshTwitterDataFailed(refreshTweetsTaskSummary);
                                });
                        else
                            refreshTwitterDataFailed(twitterAuthorizationTaskSummary);
                    });
            }
            else
            {
                this.internetNotPresentInteractionRequest.Raise(
                    new Notification()
                    {
                        Title = "Tarabica",
                        Content = "Niste konektovani na Internet."
                    },
                    n => { });
            }
        }

        private void loadTweets()
        {
            if (this.twitterDataStore.GetTweets().Count() > 0)
                HasTweets = true;

            foreach (var tweet in this.twitterDataStore.GetTweets().ToList())
                this.Tweets.Add(tweet);
        }

        private void loadConferenceData()
        {
            foreach (var track in conferenceDataStore.GetTracks())
                if (track.Id > 0)
                {
                    track.Abbeveration = track.Abbeveration.TrimEnd();
                    this.tracks.Add(track);
                }


            //foreach (var _event in conferenceDataStore.GetEvents())
            //    if (_event.Visibility == true && DateTime.Now < _event.TimeBegin)
            //        this.events.Add(_event);

            //if (this.events.Count > 0)
            //    this.currentEvent.Add(this.events[0]);

            this.RaisePropertyChanged(() => this.Tracks);
        }

        //public bool EventVisible
        //{
        //    get
        //    {
        //        return this.events.Count>0;
        //    }
        //}

        public bool HasTarabicaStarted
        {
            get
            {
                return DateTime.Now > this.startTime;
            }           
        }

        public bool HasTweets
        {
            get
            {
                return hasTweets;
            }
            set
            {
                hasTweets = value;
                this.RaisePropertyChanged(() => HasTweets);
            }
        }

        public bool HasFavourites
        {
            get
            {
                return this.conferenceDataStoreLocator.GetStore().HasFavourites();
            }
        }

        public int SelectedPivotIndex
        {
            get 
            {
                if (selectedPivotIndex == 2 && this.hasTweets && !isTwitterInitiallyRefreshed && !this.isSyncing && !this.isTweetsComing) // twitter tab
                {
                    getNewTweets();      
                }
                return this.selectedPivotIndex; 
            }
            set
            {
                this.selectedPivotIndex = value;
                this.RaisePropertyChanged(() => this.SelectedPivotIndex);
                RaisePropertyChanged(() => SelectedText);
                RaisePropertyChanged(() => IsSocialTabActive);
            }
        }

        public string SelectedText
        {
            get
            {
                switch (this.selectedPivotIndex)
                {
                    case 0:
                        return "Home";
                    case 1:
                        return "Tracks";
                    case 2:
                        return "Social";
                    default:
                        return String.Empty;
                }
            }
        }

        public bool IsSocialTabActive
        {
            get { return this.selectedPivotIndex == 2; }
        }
          
        public IInteractionRequest ConfErrorInteractionRequest
        {
            get { return this.confErrorInteractionRequest; }
        }

        public IInteractionRequest TwitterErrorInteractionRequest
        {
            get { return this.twitterErrorInteractionRequest; }
        }

        public IInteractionRequest InternetNotPresentInteractionRequest
        {
            get { return this.internetNotPresentInteractionRequest; }
        }

        private void alreadyHaveTheNewestVersion()
        {
            IsSynchronizing = false;
        }

        private void webServiceError()
        {
            this.confErrorInteractionRequest.Raise(
                new Notification()
                {
                    Title = "Tarabica",
                    Content = "Došlo je do greške prilikom preuzimanja konferencijskih podataka."
                },
                n => { });
            IsSynchronizing = false;
        }

        private void newConferenceDataArrived()
        {
            IsSynchronizing = false;

             /* REFRESH EVENTS BEGIN */
            //if (timer.IsEnabled)
            //    timer.Stop();

            //this.events.Clear(); // clear active events list
            //this.CurrentEvent.Clear(); // remove currently displayed event from screen

            // populate the active events list with new active events
            // use confDataStoreLocator to get the newest data
            //foreach (var _event in conferenceDataStoreLocator.GetStore().GetEvents())
            //    if (_event.Visibility == true && DateTime.Now < _event.TimeBegin)
            //        this.events.Add(_event);
            
            // restart active events shuffling
            //this.currentEventIndex = 0;
            //if (this.events.Count > 0)
            //    this.currentEvent.Add(this.events[0]);

            // reenable timer if it was prevouisly stopped and has work to do (countdown, or event shuffling)
            //if (!timer.IsEnabled && (!HasSinergijaStarted || this.events.Count > 1))
            //    timer.Start();
            /* REFRESH EVENTS END */

            //this.newConfDataInteractionRequest.Raise(
            //    new Notification()
            //    {
            //        Title = "Agenda je osvežena.",
            //    },
            //    action => new ToastPopupAction() { PopupElementName = "NewConferenceDataPopup", IsEnabled = true });
        }

        private void loadNewTweets()
        {
            IsTweetsComing = true;
            IsSynchronizing = false;

            var currentTweets = this.twitterDataStore.GetTweets().ToList();
            var oldTweets = this.tweets.ToList();

            var newTweets = currentTweets.Except(oldTweets, new TweetEqualityComparer()).OrderBy(t => t.Time).ToList();
            
            if (newTweets.Count > 0 && HasTweets == false)
            {
                HasTweets = true;
            }

            var e = Enumerable.Range(0, newTweets.Count);
            var sequence = Observable.Interval(TimeSpan.FromSeconds(1.5)).Zip(e.ToObservable(), (tick, index) => index);

            sequence.ObserveOn(SynchronizationContext.Current).Subscribe(
                index =>
                {

                    this.Tweets.Insert(0, newTweets[index]);
                },
                () =>
                {
                    IsTweetsComing = false;
                });
        }

        private void loadOlderTweets()
        {
            IsTweetsComing = true;
            IsSynchronizing = false;

            var currentTweets = this.twitterDataStore.GetTweets().ToList();
            var oldTweets = this.tweets.ToList();

            var newTweets = currentTweets.Except(oldTweets, new TweetEqualityComparer()).OrderByDescending(t => t.Time).ToList();

            var e = Enumerable.Range(0, newTweets.Count);
            var sequence = Observable.Interval(TimeSpan.FromSeconds(1.5)).Zip(e.ToObservable(), (tick, index) => index);

            sequence.ObserveOn(SynchronizationContext.Current).Subscribe(
                index =>
                {
                    this.Tweets.Add(newTweets[index]);
                },
                () =>
                {
                    IsTweetsComing = false;
                });
        }

        public ObservableCollection<Tweet> Tweets
        {
            get
            {
                return tweets;
            }
        }

        //public ObservableCollection<Tarabica.DataModel.Models.Conf.Event> CurrentEvent
        //{
        //    get
        //    {
        //        return currentEvent;
        //    }
        //}

        public ObservableCollection<Track> Tracks
        {
            get
            {
                return tracks;
            }
        }

        private void refreshTwitterDataFailed(TaskCompletedSummary taskSummary)
        {
            this.twitterErrorInteractionRequest.Raise(
                new Notification()
                {
                    Title = "Tarabica",
                    Content = "Došlo je do greške prilikom preuzimanja tweet-ova."
                },
                n => { });

            IsSynchronizing = false;
        }

        public bool IsSynchronizing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSynchronizing);
            }
        }

        public bool IsTweetsComing
        {
            get { return this.isTweetsComing; }
            set
            {
                this.isTweetsComing = value;
                this.RaisePropertyChanged(() => this.IsTweetsComing);
            }
        }

        public int MinutesLeft 
        { 
            get 
        { 
                return this.minutesLeft; 
            } 
            set 
            { 
                this.minutesLeft = value; 
                RaisePropertyChanged(() => this.MinutesLeft);
                RaisePropertyChanged(() => this.MinutesText); 
            }    
        }

        public int HoursLeft 
        { 
            get 
            { 
                return this.hoursLeft; 
            } 
            set 
            { 
                this.hoursLeft = value; 
                RaisePropertyChanged(() => this.HoursLeft);
                RaisePropertyChanged(() => this.HoursText);
            } 
        }

        public int DaysLeft 
        { 
            get 
            { 
                return this.daysLeft; 
            } 
            set 
            { 
                this.daysLeft = value; 
                RaisePropertyChanged(() => this.DaysLeft);
                RaisePropertyChanged(() => this.DaysText);
            } 
        }

        public string MinutesText
        {
            get
            {
                if (MinutesLeft % 10 == 1 && MinutesLeft != 11)
                    return "minut";
                else
                    return "minuta";
            }
            set
            {
                this.minutesText = value; 
                RaisePropertyChanged(() => this.MinutesText);
            }
        }

        public string HoursText
        {
            get
            {
                switch (HoursLeft)
                {
                    case 1:
                    case 21:
                        return "sat";
                    case 2:
                    case 3:
                    case 4:
                    case 22:
                    case 23:
                        return "sata";
                    default:
                        return "sati";
                }
            }
            set
            {
                this.hoursText = value; 
                RaisePropertyChanged(() => this.HoursText);
            }
        }

        public string DaysText
        {
            get
            {
                if (DaysLeft % 10 == 1 && DaysLeft != 11)
                    return "dan";
                else
                    return "dana";
            }
            set
            {
                this.daysText = value; 
                RaisePropertyChanged(() => this.DaysText);
            }
        }

        public override void IsBeingActivated()
        {
            this.SelectedPivotIndex = Tombstoning.Load<int>("MainPanoramaIndex");
            // it timer is not running and it should be running (for sinergija countdown or event shuffling)
            if (!timer.IsEnabled && !HasTarabicaStarted)
                timer.Start();
        }

        public override void IsBeingDeactivated()
        {
            Tombstoning.Save("MainPanoramaIndex", this.SelectedPivotIndex);
            if (timer.IsEnabled)
                timer.Stop();
        }

        public override void IsBeingObscured(bool isLocked)
        {
            if (timer.IsEnabled)
                timer.Stop();
        }

        public override void IsBeingUnobscured()
        {
            // it timer is not running and it should be running (for sinergija countdown or event shuffling)
            if (!timer.IsEnabled && !HasTarabicaStarted)
                timer.Start();
        }
    }
}

using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tarabica.DataServices.Store.Conference;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Tarabica.WP7App.Services;
using Tarabica.WP7App.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Tarabica.Model.Domain.Conference;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Tarabica.WP7App.ViewModels
{
    public class SpeakerDetailsViewModel : ViewModel
    {
        private readonly IConferenceDataStoreLocator conferenceDataStoreLocator;

        private ObservableCollection<GroupList<SessionInfoViewModel>> speakerSessionsObservableCollection;
        private CollectionViewSource speakerSessionsViewSource;

        public SpeakerDetailsViewModel(
            IConferenceDataStoreLocator conferenceDataStoreLocator,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.conferenceDataStoreLocator = conferenceDataStoreLocator;
            initialize();
        }

        private void initialize()
        {
            this.SessionSelectedCommand = new DelegateCommand<SessionInfoViewModel>(
                s =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SessionDetailsView.xaml?ID=" + s.Id, UriKind.Relative));
                });

            this.ToggleFavCommand = new DelegateCommand<SessionInfoViewModel>(
                s =>
                {
                    s.Session.IsFavourite = !s.IsFavourite;
                    conferenceDataStoreLocator.GetStore().SaveFavouritesStore();
                    RaisePropertyChanged(() => SpeakerSessionInfos);
                });

            this.PageLoadedCommand = new DelegateCommand(
                () =>
                {
                    RaisePropertyChanged(() => SpeakerSessionInfos);
                });

            this.IsBeingActivated();
        }

        private int? id;
        private Speaker speaker;
        public Speaker Speaker
        {
            get
            {
                var id = int.Parse(NavigationService.CurrentSource
                                       .OriginalString.Split('=')[1]);
                if (this.id == null || (this.id != id))
                {
                    this.id = id;
                    speaker = conferenceDataStoreLocator.GetStore().FindSpeaker(id);
                }
                return speaker;
            }
        }

        private static string SlotInfo(Session session)
        {
            return string.Format("{0}", session.Slot.ToTimeSpanString());
        }

        private List<GroupList<SessionInfoViewModel>> groupSessions(IEnumerable<Session> sessions)
        {
            var result = new List<GroupList<SessionInfoViewModel>>();
            sessions = sessions.Where(s => s.Slot != null);
            var slots = sessions.OrderBy(s => s.Slot.Day.Date).ThenBy(s => s.Slot.ToTimeSpanString()).ThenBy(s => s.Track.Abbeveration).Select(s => SlotInfo(s)).Distinct();
            var sessionInfos = sessions.OrderBy(s => s.Slot.Day.Date).ThenBy(s => s.Slot.ToTimeSpanString()).ThenBy(s => s.Track.Abbeveration).Select(s => new SessionInfoViewModel(s, NavigationService));
            var groups = new Dictionary<string, GroupList<SessionInfoViewModel>>();
            foreach (var slot in slots)
            {
                var group = new GroupList<SessionInfoViewModel>(slot);
                result.Add(group);
                groups[slot] = group;
            }
            foreach (var sessionInfo in sessionInfos)
            {
                groups[SlotInfo(sessionInfo.Session)].Add(sessionInfo);
            }
            return result;
        }

        public string FullName
        {
            get
            {
                return Speaker.FullName.Replace(" ",
                    System.Environment.NewLine);
            }
        }
        public string Company { get { return Speaker.Company; } }

        public string Bio
        {
            get
            {
                return Speaker.Bio.Replace("\r\n", String.Empty);
            }
        }

        public BitmapImage Picture
        {
            get
            {
                var image = new BitmapImage(new Uri(Speaker.PictureUrl, UriKind.RelativeOrAbsolute));
                image.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                return image;
            }
        }

        public ICollectionView SpeakerSessionInfos
        {
            get
            {
                speakerSessionsObservableCollection = new ObservableCollection<GroupList<SessionInfoViewModel>>();
                var speakerSessionInfoViewModelGroups = groupSessions(Speaker.Sessions);
                speakerSessionInfoViewModelGroups.ForEach(speakerSessionsObservableCollection.Add);

                speakerSessionsViewSource = new CollectionViewSource { Source = speakerSessionsObservableCollection };
                speakerSessionsViewSource.View.MoveCurrentToPrevious();
                return speakerSessionsViewSource.View;
            }
        }

        public DelegateCommand<SessionInfoViewModel> SessionSelectedCommand { get; set; }
        public DelegateCommand<SessionInfoViewModel> ToggleFavCommand { get; set; }
        public DelegateCommand PageLoadedCommand { get; set; }
        public override void IsBeingActivated()
        { }
    }
}

using System.Reactive;
using Microsoft.Practices.Prism.Commands;
using Tarabica.Model.Domain.Conference;
using Tarabica.WP7App.Infrastructure;
using Tarabica.WP7App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Tarabica.DataServices.Store.Conference;

namespace Tarabica.WP7App.ViewModels
{
    public class SpeakerListViewModel : ViewModel
    {
        private readonly IConferenceDataStoreLocator conferenceDataStoreLocator; //ctor param

        private ObservableCollection<GroupList<SpeakerInfoViewModel>> speakersObservableCollection;
        private CollectionViewSource speakersViewSource;

        public SpeakerListViewModel(
            IConferenceDataStoreLocator conferenceDataStoreLocator,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.conferenceDataStoreLocator = conferenceDataStoreLocator;
            initialize();
        }

        private void initialize()
        {
            speakersViewSource = new CollectionViewSource();

            this.SpeakerSelectedCommand = new DelegateCommand<SpeakerInfoViewModel>(
                s =>
                {
                    NavigationService.Navigate(
                        new Uri("/Views/SpeakerDetailsView.xaml?Id=" + s.Id, UriKind.Relative));
                });

            this.PageLoadedCommand = new DelegateCommand(
            () =>
            {
                //refreshSpeakers();
            });

            this.IsBeingActivated();
        }

        public DelegateCommand<SpeakerInfoViewModel> SpeakerSelectedCommand { get; set; }
        public DelegateCommand PageLoadedCommand { get; set; }

        public ICollectionView Speakers
        {
            get
            {
                //refreshSpeakers();
                return speakersViewSource.View;
            }
        }

        private void refreshSpeakers()
        {
            speakersObservableCollection = new ObservableCollection<GroupList<SpeakerInfoViewModel>>();
            var speakerGroups = groupSpeakers(conferenceDataStoreLocator.GetStore().GetSpeakers());

            speakerGroups.ForEach(speakersObservableCollection.Add);            
        }

        private List<GroupList<SpeakerInfoViewModel>> groupSpeakers(IEnumerable<Speaker> speakers)
        {
            var result = new List<GroupList<SpeakerInfoViewModel>>();
            var groupHeaders = new string[] { "a", "b", "c", "č", "ć", "d", "đ", "e", "f", "g", "h", "i", "j", "k", "l", "lj", "m", "n", "nj", "o", "p", "q", "r", "s", "š", "t", "u", "v", "w", "x", "y", "z", "ž" };
            var people = speakers.OrderBy(s => s.LastName).Select(s => new SpeakerInfoViewModel(s, NavigationService));
            var groups = new Dictionary<string, GroupList<SpeakerInfoViewModel>>();
            
            foreach (string header in groupHeaders)
            {
                var group = new GroupList<SpeakerInfoViewModel>(header);
                result.Add(group);
                groups[header] = group;
            }

            foreach (var person in people)
            {
                var lastName = person.LastName.Trim().ToLower();
                var groupName = lastName.Substring(0, 2);
                if (groupName == "lj" || groupName == "nj")
                {
                    groups[groupName].Add(person);
                }
                else
                {
                    groups[lastName.Substring(0, 1)].Add(person);
                }
            }

            return result;
        }

        public override void HandleOnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            refreshSpeakers();
            speakersViewSource = new CollectionViewSource { Source = speakersObservableCollection };
            RaisePropertyChanged(() => Speakers);

            //var o = Observable.Start(() => refreshSpeakers()).ObserveOnDispatcher().Subscribe(
            //    _ => 
            //    {
            //        speakersViewSource = new CollectionViewSource { Source = speakersObservableCollection }; 
            //        RaisePropertyChanged(() => Speakers);
            //    });      
        }

        public override void IsBeingActivated()
        {}
    }
}

using Microsoft.Phone.Tasks;
using Microsoft.Practices.Prism.Commands;
using Tarabica.WP7App.Services;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;

namespace Tarabica.WP7App.ViewModels
{
    public class ConferenceInfoViewModel : ViewModel
    {

        public ConferenceInfoViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            initialize();
        }

        private void initialize()
        {
            this.BingMapCommand = new DelegateCommand(
                () =>
                {
                    BingMapsTask bingMapsTask = new BingMapsTask();
                    bingMapsTask.Center = new GeoCoordinate(44.757522, 20.496736);
                    bingMapsTask.SearchTerm = "Kumodraška 261 Beograd";
                    bingMapsTask.ZoomLevel = 2;

                    bingMapsTask.Show();
                });

            this.EmailCommand = new DelegateCommand(
                () =>
                {
                    EmailComposeTask emailComposeTask = new EmailComposeTask();
                    emailComposeTask.To = "marina.nikolic@tarabica.org";

                    emailComposeTask.Show();
                });
        }

        public DelegateCommand BingMapCommand { get; set; }
        public DelegateCommand EmailCommand { get; set; }


        public override void IsBeingActivated()
        {}
    }
}

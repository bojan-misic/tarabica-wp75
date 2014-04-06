using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.Model.Domain.Conference;
using Tarabica.WP7App.Services;
using System.Windows.Media.Imaging;

namespace Tarabica.WP7App.ViewModels
{
    public class SpeakerInfoViewModel : ViewModel
    {
        private readonly Speaker speaker;

        public SpeakerInfoViewModel(
            Speaker speaker,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.speaker = speaker;
        }

        public Speaker Speaker { get { return speaker; } }

        public int Id { get { return Speaker.Id; } }
        public string FullName { get { return Speaker.FullName.Replace(" ", System.Environment.NewLine); } }
        public string FirstName { get { return Speaker.FirstName; } }
        public string LastName { get { return Speaker.LastName; } }
        public string Company { get { return Speaker.Company; } }
        
        public List<Track> Tracks
        {
            get
            {
                return speaker.Tracks;
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

        public override void IsBeingActivated()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.Model.Domain.Conference;

namespace Tarabica.Model
{
    public class StaticData
    {
        public static List<Track> GetTracks()
        {
            List<Track> tracks = new List<Track>();

            // default
            //tracks.Add(new Track()
            //{
            //    Id = -1,
            //    Abbeveration = "UNK",
            //    Title = "Unknown",
            //    Description = String.Empty,
            //    //ImageUri = "/Resources/Images/DEV.png"
            //});

            // designator for all tracks
            //tracks.Add(new Track()
            //{
            //    Id = -1,
            //    Abbeveration = "ALL",
            //    Title = "All tracks",
            //    Description = String.Empty,
            //    //ImageUri = "/Resources/Images/DEV.png"
            //});

            tracks.Add(new Track()
            {
                Id = 1,
                Abbeveration = "ALM",
                Title = "Application Lifecycle & Project Management",
                Description = String.Empty,
                PictureUrl = "/Assets/ALM.png"
            });

            tracks.Add(new Track()
            {
                Id = 13,
                Abbeveration = "APM",
                Title = "Agile i upravljanje projektima",
                Description = String.Empty,
                PictureUrl = "/Assets/APM.png"
            });

            tracks.Add(new Track()
            {
                Id = 2,
                Abbeveration = "CLD",
                Title = "Cloud, Azure app Development",
                Description = String.Empty,
                PictureUrl = "/Assets/CLD.png"
            });

            tracks.Add(new Track()
            {
                Id = 3,
                Abbeveration = "DBI",
                Title = "Data Platform & Business Intelligence",
                Description = String.Empty,
                PictureUrl = "/Assets/DBI.png"
            });

            tracks.Add(new Track()
            {
                Id = 4,
                Abbeveration = "DEV",
                Title = "Desktop Languages, Frameworks, Developer Tools",
                Description = String.Empty,
                PictureUrl = "/Assets/DEV.png"
            });


            tracks.Add(new Track()
            {
                Id = 9,
                Abbeveration = "DVC",
                Title = "Windows 8, Windows Phone & Mobile Services",
                Description = String.Empty,
                PictureUrl = "/Assets/DVC.png"
            });

            tracks.Add(new Track()
            {
                Id = 7,
                Abbeveration = "DYN",
                Title = "Dynamics",
                Description = String.Empty,
                PictureUrl = "/Assets/DYN.png"
            });

            tracks.Add(new Track()
            {
                Id = 8,
                Abbeveration = "GAM",
                Title = "Game Development",
                Description = String.Empty,
                PictureUrl = "/Assets/GAM.png"
            });

            tracks.Add(new Track()
            {
                Id = 11,
                Abbeveration = "MSC",
                Title = "Other",
                Description = String.Empty,
                PictureUrl = "/Assets/MSC.png"
            });

            tracks.Add(new Track()
            {
                Id = 5,
                Abbeveration = "SES",
                Title = "SharePoint, Office 365 & Enterprise Social",
                Description = String.Empty,
                PictureUrl = "/Assets/SES.png"
            });

            tracks.Add(new Track()
            {
                Id = 6,
                Abbeveration = "SRV",
                Title = "Windows Server, Networks, Cloud Platform and Modern Datacenter",
                Description = String.Empty,
                PictureUrl = "/Assets/SRV.png"
            });

            tracks.Add(new Track()
            {
                Id = 12,
                Abbeveration = "UX ",
                Title = "Korisnicko iskustvo",
                Description = String.Empty,
                PictureUrl = "/Assets/UX.png"
            });

            tracks.Add(new Track()
            {
                Id = 10,
                Abbeveration = "WEB",
                Title = "Web Development",
                Description = String.Empty,
                PictureUrl = "/Assets/WEB.png"
            });

            return tracks;
        }
    }
}

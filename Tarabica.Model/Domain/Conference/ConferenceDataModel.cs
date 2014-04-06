using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Tarabica.Model.Dto.Conference;

namespace Tarabica.Model.Domain.Conference
{
    public class ConferenceDataModel
    {
        public int? Version { get; set; }
        public Dictionary<int, Day> Days { get; set; }
        public Dictionary<int, Room> Rooms { get; set; }
        public Dictionary<int, Session> Sessions { get; set; }
        public Dictionary<int, Slot> Slots { get; set; }
        public Dictionary<int, Speaker> Speakers { get; set; }
        public Dictionary<int, Track> Tracks { get; set; }
        //public Dictionary<int, Event> Events { get; set; }

        public bool IsInitialized
        {
            get { return Version != null; }
        }

        public static ConferenceDataModel FromDto(ConferenceDataModelDto dtoModel)
        {
            var model = new ConferenceDataModel();

            // if dtoModel is not initialized return an empty model
            if (dtoModel.Sessions == null) return model;

            model.Version = dtoModel.Version;
            loadDays(dtoModel, model);
            loadRooms(dtoModel, model);
            loadTracks(dtoModel, model);
            loadSlots(dtoModel, model);
            loadSpeakers(dtoModel, model);
            loadSessions(dtoModel, model);
            //loadEvents(dtoModel, model);
            makeSessionSpeakerRelations(dtoModel, model);

            return model;
        }

        private static void loadDays(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Days = new Dictionary<int, Day>();

            // add default value
            model.Days.Add(-1, new Day() { Id = -1, Date = DateTime.MaxValue });

            // add values from dto
            foreach (var dayDto in dtoModel.Days.OrderBy(d => d.Date))
            {
                model.Days.Add(
                    dayDto.Id,
                    new Day() { Id = dayDto.Id, Date = dayDto.Date });
            }
        }

        private static void loadRooms(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Rooms = new Dictionary<int, Room>();

            // add default value
            model.Rooms.Add(-1, new Room() { Id = -1, Code = String.Empty });

            // add values from dto
            foreach (var roomDto in dtoModel.Rooms)
            {
                model.Rooms.Add(
                    roomDto.Id,
                    new Room() { Id = roomDto.Id, Code = roomDto.Code });
            }
        }

        private static void loadTracks(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Tracks = new Dictionary<int, Track>();

            model.Tracks.Add(-1, new Track() { Id = -1 }); // all tracks designator

            if (dtoModel.Tracks != null)
            {
                // add values from dto
                foreach (var trackDto in dtoModel.Tracks)
                {
                    model.Tracks.Add(
                        trackDto.Id,
                        new Track()
                        {
                            Id = trackDto.Id,
                            Abbeveration = trackDto.Abbeveration.TrimEnd(),
                            PictureUrl = String.IsNullOrEmpty(dtoModel.PicturesLocation) ?
                                    @"/Tarabica.WP7App;component/Assets/track-no-pic.png" :
                                        (String.IsNullOrEmpty(trackDto.PicturePath) ?
                                        @"/Tarabica.WP7App;component/Assets/track-no-pic.png" :
                                        (dtoModel.PicturesLocation + trackDto.PicturePath.Substring(1).Replace(" ", String.Empty))), // because it starts with '/'
                            Description = trackDto.Description,
                            Title = trackDto.Title.Trim()
                        }
                    );
                }
            }
            else
            {
                foreach (var track in StaticData.GetTracks())
                {
                    model.Tracks.Add(track.Id, track);
                }
            }
        }

        private static void loadSlots(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Slots = new Dictionary<int, Slot>();

            // add default value
            model.Slots.Add(-1, new Slot() { Id = -1, Day = model.Days[-1] });

            // add values from dto
            foreach (var slotDto in dtoModel.Slots.OrderBy(s => s.TimeLine))
            {
                model.Slots.Add(
                    slotDto.Id,
                    new Slot()
                    {
                        Id = slotDto.Id,
                        TimeLine = slotDto.TimeLine,
                        Day = model.Days[slotDto.DayId],
                        Type = slotDto.Type,
                        StartHour = slotDto.StartHour,
                        StartMinute = slotDto.StartMinute,
                        EndHour = slotDto.EndHour,
                        EndMinute = slotDto.EndMinute,
                        Start = new DateTime(model.Days[slotDto.DayId].Date.Year, model.Days[slotDto.DayId].Date.Month, model.Days[slotDto.DayId].Date.Day, slotDto.StartHour, slotDto.StartMinute, 0),
                        End = new DateTime(model.Days[slotDto.DayId].Date.Year, model.Days[slotDto.DayId].Date.Month, model.Days[slotDto.DayId].Date.Day, slotDto.EndHour, slotDto.EndMinute, 0)
                    });
            }
        }

        private static void loadSpeakers(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Speakers = new Dictionary<int, Speaker>();

            // add values from dto
            foreach (var speakerDto in dtoModel.Speakers)
            {
                model.Speakers.Add(
                    speakerDto.Id,
                    new Speaker()
                    {
                        Id = speakerDto.Id,
                        FirstName = speakerDto.FirstName,
                        LastName = speakerDto.LastName,
                        Company = String.IsNullOrEmpty(speakerDto.Company) ? "n/a" : HttpUtility.HtmlDecode(speakerDto.Company),
                        Bio = String.IsNullOrEmpty(speakerDto.Bio) ? "Nedostupna biografija" : HttpUtility.HtmlDecode(speakerDto.Bio),
                        PictureUrl = String.IsNullOrEmpty(dtoModel.PicturesLocation) ?
                                @"/Tarabica.WP7App;component/Assets/speaker-no-pic.png" :
                                    (String.IsNullOrEmpty(speakerDto.PictureUrl) ?
                                    @"/Tarabica.WP7App;component/Assets/speaker-no-pic.png" :
                                    (dtoModel.PicturesLocation + speakerDto.PictureUrl)),
                        Sessions = new List<Session>(),
                        Tracks = new List<Track>()
                    });
            }
        }

        private static void loadSessions(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            model.Sessions = new Dictionary<int, Session>();

            // add values from dto
            foreach (var sessionDto in dtoModel.Sessions)
            {
                var slot = model.Slots.FirstOrDefault(x => ((x.Value.Day.Id == sessionDto.DayId) && (x.Value.TimeLine == sessionDto.TimeLine))).Value;

                var trackEntry =
                    model.Tracks.Where(e => e.Value.Abbeveration == sessionDto.Track.TrimEnd())
                                .Select(e => (KeyValuePair<int, Track>?)e)
                                .FirstOrDefault();

                Track track;
                if (trackEntry != null)
                    track = trackEntry.Value.Value;
                else
                    track = model.Tracks[-1];

                model.Sessions.Add(
                    sessionDto.Id,
                    new Session()
                    {
                        Id = sessionDto.Id,
                        LanguageCode = sessionDto.Lang,
                        Level = sessionDto.Level,
                        Title = HttpUtility.HtmlDecode(sessionDto.Title),
                        Description = String.IsNullOrEmpty(sessionDto.Description) ? "Nedostupan opis" : HttpUtility.HtmlDecode(sessionDto.Description),
                        Room = model.Rooms[sessionDto.RoomId],
                        Slot = slot,
                        Track = track,
                        IsFavourite = false,
                        Speakers = new List<Speaker>()
                    });
            }
        }

        //private static void loadEvents(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        //{
        //    model.Events = new Dictionary<int, Event>();

        //    // add default value
        //    model.Events.Add(-1, new Event() { Id = -1, Name = String.Empty });

        //    // add values from dto
        //    foreach (var eventDto in dtoModel.Events)
        //    {
        //        model.Events.Add(
        //            eventDto.Id,
        //            new Event()
        //            {
        //                Id = eventDto.Id,
        //                Name = eventDto.Name,
        //                TimeBegin = eventDto.TimeBegin != default(DateTime) ? eventDto.TimeBegin.AddHours(2) : eventDto.TimeBegin, // utc compensation
        //                TimeEnd = eventDto.TimeEnd != default(DateTime) ? eventDto.TimeEnd.AddHours(2) : eventDto.TimeEnd, // utc compensation
        //                Place = eventDto.Place,
        //                Address = eventDto.Address,
        //                Description = eventDto.Description,
        //                Note = eventDto.Note,
        //                Longitude = eventDto.Longitude,
        //                Latitude = eventDto.Latitude,
        //                Visibility = eventDto.Visibility
        //            });
        //    }
        //}

        private static void makeSessionSpeakerRelations(ConferenceDataModelDto dtoModel, ConferenceDataModel model)
        {
            foreach (var relation in dtoModel.SessionSpeakerRelations)
            {
                var session = model.Sessions[relation.SessionId];
                var speaker = model.Speakers[relation.SpeakerId];

                session.Speakers.Add(speaker);
                speaker.Sessions.Add(session);

                if (!speaker.Tracks.Contains(session.Track) && session.Track.Id > 0)
                    speaker.Tracks.Add(session.Track);
            }
        }      
    }
}

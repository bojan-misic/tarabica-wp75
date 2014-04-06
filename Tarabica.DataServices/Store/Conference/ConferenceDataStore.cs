using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Tarabica.Model.Domain.Conference;
using Tarabica.Model.Dto.Conference;
using Tarabica.DataServices.Store;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Tarabica.DataServices.Http.Conference;
using Tarabica.DataServices.Tasks;
using System.Net;

namespace Tarabica.DataServices.Store.Conference
{
    public class ConferenceDataStore : Store, IConferenceDataStore
    {
        private const string favouritesStoreName = "FavouritesStore";
        private const string conferenceDataStoreName = "ConferenceDataStore";
        private const string checkingVersionTaskName = "CheckingVersionTask";
        private const string refreshingConferenceDataTaskName = "RefreshingDataTask";

        private IConferenceDataService conferenceDataService;
        private ConferenceDataModel conferenceDataModel;

        public int Version { get { return conferenceDataModel.Version ?? 0; } }

        public ConferenceDataStore(IConferenceDataService conferenceDataService)
        {
            this.conferenceDataService = conferenceDataService;
            var favourites = GetDataFromJson<List<int>>(favouritesStoreName);

            conferenceDataModel = ConferenceDataModel.FromDto(GetDataFromJson<ConferenceDataModelDto>(conferenceDataStoreName));
            if (!conferenceDataModel.IsInitialized)
            {
                conferenceDataModel = ConferenceDataModel.FromDto(loadDefaultModel());
            }

            foreach (var session in conferenceDataModel.Sessions.Values)
            {
                if (favourites.Contains(session.Id)) session.IsFavourite = true;
            }
        }

        public IObservable<TaskCompletedSummary> CheckIfNewVersionIsAvailable()
        {
            return conferenceDataService.GetVersion().Select(
                version =>
                {
                    if (version > this.Version)
                    {
                        return new TaskCompletedSummary
                        {
                            Task = checkingVersionTaskName,
                            Result = TaskSummaryResult.NewVersionAvailable,
                            Context = version.ToString()
                        };
                    }
                    else
                    {
                        return new TaskCompletedSummary
                        {
                            Task = checkingVersionTaskName,
                            Result = TaskSummaryResult.AlreadyHaveTheCurrentVersion,
                            Context = version.ToString()
                        };
                    }
                }).Catch(
                        (Exception exception) =>
                        {
                            if (exception is WebException)
                            {
                                var webException = exception as WebException;
                                var summary = ExceptionHandling.GetSummaryFromWebException(checkingVersionTaskName, webException);
                                return Observable.Return(summary);
                            }

                            if (exception is UnauthorizedAccessException)
                            {
                                return Observable.Return(new TaskCompletedSummary { Task = checkingVersionTaskName, Result = TaskSummaryResult.AccessDenied });
                            }

                            ;
                            return Observable.Return(
                                new TaskCompletedSummary { Task = checkingVersionTaskName, Result = TaskSummaryResult.UnknownError }
                                );
                        });
        }

        public IObservable<TaskCompletedSummary> RefreshConferenceData()
        {
            return conferenceDataService.GetConferenceData().Select(
                conferenceDataModelDto =>
                {
                    var result = saveConferenceData(conferenceDataModelDto);

                    return new TaskCompletedSummary
                    {
                        Task = refreshingConferenceDataTaskName,
                        Result = TaskSummaryResult.Success,
                        Context = result.ToString()
                    };                  
                }).Catch(
                        (Exception exception) =>
                        {
                            if (exception is WebException)
                            {
                                var webException = exception as WebException;
                                var summary = ExceptionHandling.GetSummaryFromWebException(checkingVersionTaskName, webException);
                                return Observable.Return(summary);
                            }

                            if (exception is UnauthorizedAccessException)
                            {
                                return Observable.Return(new TaskCompletedSummary { Task = checkingVersionTaskName, Result = TaskSummaryResult.AccessDenied });
                            }

                            ;
                            return Observable.Return(
                                new TaskCompletedSummary { Task = checkingVersionTaskName, Result = TaskSummaryResult.UnknownError }
                                );
                        });
        }

        public IEnumerable<Session> GetSessionsByDayIndexAndTrack(int index, string track)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Session> GetSessionsByTrack(string track)
        {
            return conferenceDataModel.Sessions.Values.Where(s => s.Track.Abbeveration == track);
        }

        public IEnumerable<Session> GetSessionsByDay(int dayId)
        {
            return conferenceDataModel.Sessions.Values.Where(s => s.Slot != null && s.Slot.Day.Id == dayId);
        }

        public IEnumerable<Session> GetFavouriteSessions()
        {
            return conferenceDataModel.Sessions.Values.Where(s => s.IsFavourite);
        }

        public IEnumerable<Session> GetNextSessions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Speaker> GetSpeakers()
        {
            return conferenceDataModel.Speakers.Values;
        }

        public IEnumerable<Track> GetTracks()
        {
            return conferenceDataModel.Tracks.Values;
        }

        //public IEnumerable<Sinergija13.DataModel.Models.Conference.Event> GetEvents()
        //{
        //    return conferenceDataModel.Events.Values;
        //}

        public IEnumerable<Day> GetDays()
        {
            return conferenceDataModel.Days.Values;
        }

        public Speaker FindSpeaker(int id)
        {
            return conferenceDataModel.Speakers[id];
        }

        public Session FindSession(int id)
        {
            return conferenceDataModel.Sessions[id];
        }

        public Day FindDay(int id)
        {
            return conferenceDataModel.Days[id];
        }

        public Track FindTrack(int id)
        {
            return conferenceDataModel.Tracks[id];
        }

        public Room FindRoom(int id)
        {
            return conferenceDataModel.Rooms[id];
        }

        //public Sinergija13.DataModel.Models.Conference.Event FindEvent(int id)
        //{
        //    return conferenceDataModel.Events[id];
        //}

        public void SaveFavouritesStore()
        {
            StoreDataToJson(favouritesStoreName, this.conferenceDataModel.Sessions.Values.Where(s => s.IsFavourite).Select(s => s.Id).ToList());
        }

        private bool saveConferenceData(ConferenceDataModelDto newConferenceDataModelDto)
        {
            if (newConferenceDataModelDto.Version != 0)
            {
                this.conferenceDataModel = ConferenceDataModel.FromDto(newConferenceDataModelDto);
                var favourites = GetDataFromJson<List<int>>(favouritesStoreName);

                foreach (var session in this.conferenceDataModel.Sessions.Values)
                {
                    if (favourites.Contains(session.Id)) session.IsFavourite = true;
                }

                StoreDataToJson(conferenceDataStoreName, newConferenceDataModelDto);
                
                return true;
            }
            return false;
        }

        public bool HasFavourites()
        {
            foreach (var session in this.conferenceDataModel.Sessions.Values)
                if (session.IsFavourite)
                    return true;

            return false;
        }

        private ConferenceDataModelDto loadDefaultModel()
        {
            using (var str = new StreamReader(Application.GetResourceStream(new Uri("/Tarabica.DataServices;component/defaultConferenceDataModelDto.json", UriKind.Relative)).Stream))
            {
                return JsonConvert.DeserializeObject<ConferenceDataModelDto>(str.ReadToEnd());
            }
        }
    }
}

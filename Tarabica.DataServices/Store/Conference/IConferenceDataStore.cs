using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.DataServices.Tasks;
using Tarabica.Model.Domain.Conference;

namespace Tarabica.DataServices.Store.Conference
{
    public interface IConferenceDataStore
    {
        int Version { get; }

        IEnumerable<Session> GetSessionsByDayIndexAndTrack(int index, string track);
        IEnumerable<Session> GetSessionsByTrack(string track);
        IEnumerable<Session> GetSessionsByDay(int dayId);
        IEnumerable<Session> GetFavouriteSessions();
        IEnumerable<Session> GetNextSessions();
        IEnumerable<Speaker> GetSpeakers();
        IEnumerable<Track> GetTracks();
        //IEnumerable<Event> GetEvents();
        IEnumerable<Day> GetDays();

        Speaker FindSpeaker(int id);
        Session FindSession(int id);
        Day FindDay(int id);
        Track FindTrack(int id);
        Room FindRoom(int id);
        //Event FindEvent(int id);


        bool HasFavourites();
        void SaveFavouritesStore();

        IObservable<TaskCompletedSummary> CheckIfNewVersionIsAvailable();
        IObservable<TaskCompletedSummary> RefreshConferenceData();

        //bool SaveConfData(ConfDataModelDto confDataDto);  
    }
}

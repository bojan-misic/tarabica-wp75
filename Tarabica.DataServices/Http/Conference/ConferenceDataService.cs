using System;
using System.Net;
using System.Reactive.Linq;
using SharpGIS;
using Tarabica.Model.Dto.Conference;

namespace Tarabica.DataServices.Http.Conference
{
    public class ConferenceDataService : IConferenceDataService
    {
        private string serviceUriString = "http://tarabica.msforge.net/Device/";
        private const string agendaParameter = "GetData";
        private const string versionParameter = "GetVersion";

        public IObservable<ConferenceDataModelDto> GetConferenceData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequestCreator.GZip.Create(new Uri(serviceUriString + agendaParameter));
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            return request.GetJson<ConferenceDataModelDto>();
        }

        public IObservable<int> GetVersion()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(serviceUriString + versionParameter));
            return request.GetJson<int>();
        }
    }
}

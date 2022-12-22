using Newshore.travelConnection.Domain.Entity.ExternalResponseModels;
using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Infrastructure.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newshore.travelConnection.Domain.Interface;

namespace Newshore.travelConnection.Domain.Core
{
    public class ServiceNewshoreAirDomain: IServiceNewshoreAirDomain
    {
        private readonly IJourneyRepository _journeyRepository;

        public ServiceNewshoreAirDomain(IJourneyRepository journeyRepository)
        {
            _journeyRepository = journeyRepository;
        }

        public async Task<string>  GetServiceNewshore(Task<string> url)
        {
            string responseBody = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url.Result.ToString());
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return null;
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }

        public  async Task<List<Flight>> ConvertlistToObjet(Task<string>? response)
        {
            List<Flight> listFlight = new List<Flight>();

            var responseConvert = JsonConvert.DeserializeObject<IEnumerable<RecruitingAPI>>(response.Result.ToString());
            foreach (var item in responseConvert)
            {
                Flight flight = new Flight();

                Transport trasport = new Transport();
                trasport.FlightCarrier = item.flightCarrier;
                trasport.FlightNumber = item.flightNumber;

                flight.transport = trasport;
                flight.Destination = item.departureStation;
                flight.Origin = item.arrivalStation;
                flight.Price = Convert.ToDouble(item.price);
                listFlight.Add(flight);
            }

            return listFlight;
        }
        public async Task<string> GetUrlApiNewshore(IEnumerable<dynamic>? listServiceNewshore)
        {
            string urlApiNewshore = string.Empty;

            foreach (var item in listServiceNewshore)
                urlApiNewshore = item.url;

            return urlApiNewshore;
        }

    }
}

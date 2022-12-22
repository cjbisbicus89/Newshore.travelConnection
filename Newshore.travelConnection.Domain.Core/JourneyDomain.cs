using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.ExternalResponseModels;
using Newshore.travelConnection.Domain.Entity.Response;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Infrastructure.Interface;
using Newtonsoft.Json;
using System.Net;

namespace Newshore.travelConnection.Domain.Core
{
    public class JourneyDomain:IJourneyDomain
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IServiceNewshoreAirDomain _serviceNewshoreAirDomain;

        public JourneyDomain(IJourneyRepository journeyRepository, IServiceNewshoreAirDomain serviceNewshoreAirDomain)
        {
            _journeyRepository = journeyRepository;
            _serviceNewshoreAirDomain = serviceNewshoreAirDomain;
        }
        #region Asynchronous Methods

        public async Task<Response<dynamic>> GetListFlightsAsync()
        {

            var serviceApiNewshore = await _journeyRepository.GetListFlightsAsync();
            var urlServiceApiNewshore = _serviceNewshoreAirDomain.GetUrlApiNewshore(serviceApiNewshore);

            Task<string>? responseApiNewshore = _serviceNewshoreAirDomain.GetServiceNewshore(urlServiceApiNewshore);
            var listFlights = _serviceNewshoreAirDomain.ConvertlistToObjet(responseApiNewshore);

            return new Response<dynamic>() { success = true, error = false, result = listFlights };
        }

        public async Task<Response<dynamic>> GetJourneyByOriginAndDestination(string origin, string destination )
        {
            var serviceApiNewshore = await _journeyRepository.GetListFlightsAsync();
            var urlServiceApiNewshore = _serviceNewshoreAirDomain.GetUrlApiNewshore(serviceApiNewshore);
            var responseApiNewshore = _serviceNewshoreAirDomain.GetServiceNewshore(urlServiceApiNewshore);

            List<Journey> listFlightRoute = new List<Journey>();
            var listFlightNewshore = _serviceNewshoreAirDomain.ConvertlistToObjet(responseApiNewshore);
            List<Flight> listFlight = listFlightNewshore.Result.ToList();

            var searchDirectFlights = listFlight.Where(x => x.Origin == origin 
                                                    && x.Destination == destination).ToList();
            if (searchDirectFlights.Count > 0)
            {
                foreach(var item in searchDirectFlights)
                {
                    Journey journey = new Journey();
                    journey.Origin = item.Origin;
                    journey.Destination = item.Destination;
                    journey.Price =  item.Price;
                    journey.flight = searchDirectFlights;
                    listFlightRoute.Add(journey);
                }
                
            }
            else
            {
                var flightsFilteredByOriginDestination = listFlight.Where(x => x.Origin == origin || x.Destination == destination).ToList();
                if(flightsFilteredByOriginDestination != null)
                {
                    var flightAuxiliaryList = flightsFilteredByOriginDestination;
                   
                    var searchByFirstOrigin = flightsFilteredByOriginDestination.Where(x => x.Origin== origin
                                      && x.Destination == (flightAuxiliaryList.Where(i => i.Destination == destination)
                                      .Single().Origin)).ToList();
                    var searchByFirstDestination = flightsFilteredByOriginDestination.Where(x => 
                                        x.Origin == (flightAuxiliaryList.Where(i => i.Destination == destination)
                                        .Single().Origin)
                                         && x.Destination == destination ).ToList();

                    if (searchByFirstOrigin.Count > 0 && searchByFirstDestination.Count > 0)
                    {
                        Journey journey = new Journey();
                        journey.Origin = origin;
                        journey.Destination = destination;
                        journey.flight = searchByFirstOrigin.Concat(searchByFirstDestination).ToList();
                        journey.Price = searchByFirstOrigin.Single().Price + searchByFirstDestination.Single().Price;
                        listFlightRoute.Add(journey);
                    }
                }

            }

            return new Response<dynamic>() { success = true, error = false, result = listFlightRoute };
        }

        #endregion

      


    }
}
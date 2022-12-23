using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.ExternalResponseModels;
using Newshore.travelConnection.Domain.Entity.Response;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Infrastructure.Interface;
using Newtonsoft.Json;
using System.Linq;
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
            var consultRegisteredFlights = await _journeyRepository.GetListFlightsSaveAsync(origin,destination);

            if(consultRegisteredFlights.Count() > 0)
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
                    foreach (var item in searchDirectFlights)
                    {
                        Journey journey = new Journey();
                        journey.Origin = item.Origin;
                        journey.Destination = item.Destination;
                        journey.Price = item.Price;
                        journey.flight = searchDirectFlights;
                        listFlightRoute.Add(journey);
                    }

                }
                else
                {
                    var flightsFilteredByOriginDestination = listFlight.Where(x => x.Origin == origin || x.Destination == destination).ToList();
                    if (flightsFilteredByOriginDestination != null)
                    {
                        var flightAuxiliaryList = flightsFilteredByOriginDestination;

                        List<string> listDestination = new List<string>();

                        foreach (var item in flightsFilteredByOriginDestination)
                        {
                            if (item.Origin == origin)
                                listDestination.Add(item.Destination);
                        }

                        var connectingFlights = flightsFilteredByOriginDestination
                             .Where(x => listDestination.Any(y => y == x.Origin) && x.Destination == destination).ToList();

                        var searchByFirstOrigin = flightsFilteredByOriginDestination
                            .Where(x => connectingFlights.Any(y => y.Origin == x.Destination) && x.Origin == origin).ToList();

                        var searchByFirstDestination = flightsFilteredByOriginDestination
                            .Where(x => connectingFlights.Any(y => y.Origin == x.Origin) && x.Destination == destination).ToList();

                        List<Flight> listOfOrderedFlights = new List<Flight>();
                        double priceTotal = 0;
                        foreach (var itemFirst in searchByFirstOrigin)
                        {
                            Flight flightOrderedOrigin = new Flight();
                            flightOrderedOrigin.Origin = itemFirst.Origin;
                            flightOrderedOrigin.Destination = itemFirst.Destination;
                            flightOrderedOrigin.Price = itemFirst.Price;
                            flightOrderedOrigin.transport = itemFirst.transport;
                            listOfOrderedFlights.Add(flightOrderedOrigin);
                            priceTotal = priceTotal + flightOrderedOrigin.Price;
                            foreach (var itemDestination in searchByFirstDestination)
                            {

                                if (itemDestination.Origin == flightOrderedOrigin.Destination)
                                {
                                    Flight flightOrderedDestination = new Flight();
                                    flightOrderedDestination.Origin = itemDestination.Origin;
                                    flightOrderedDestination.Destination = itemDestination.Destination;
                                    flightOrderedDestination.Price = itemDestination.Price;
                                    flightOrderedDestination.transport = itemDestination.transport;
                                    listOfOrderedFlights.Add(flightOrderedDestination);
                                    priceTotal = priceTotal + flightOrderedDestination.Price;
                                }
                            }
                        }


                        if (searchByFirstOrigin.Count > 0 && searchByFirstDestination.Count > 0)
                        {
                            Journey journey = new Journey();
                            journey.Origin = origin;
                            journey.Destination = destination;
                            journey.flight = listOfOrderedFlights;
                            journey.Price = priceTotal;
                            listFlightRoute.Add(journey);
                        }
                    }

                }

                if (listFlightRoute.Count == 0)
                {
                    return new Response<dynamic>() { success = false, error = false, result = listFlightRoute };
                }
                else
                {

                    return new Response<dynamic>() { success = true, error = false, result = listFlightRoute };
                }
            }
            else
            {
                return new Response<dynamic>() { success = true, error = false, result = null };
            }




            
        }

        #endregion

      


    }
}
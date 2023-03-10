using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.ExternalResponseModels;
using Newshore.travelConnection.Domain.Entity.Response;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Infrastructure.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

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

            if(consultRegisteredFlights.Count() == 0)
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
                    var responseSaveJourneyFligh = await PersistConsultedFlights(listFlightRoute);
                    return new Response<dynamic>() { success = true, error = false, result = listFlightRoute };
                }
            }
            else
            {
                var responsePersistQuery = await GetPersistConsultedFlights(consultRegisteredFlights);
                return new Response<dynamic>() { success = true, error = false, result = responsePersistQuery.result };
            }

        }

        #endregion
        public async Task<Response<dynamic>> PersistConsultedFlights(List<Journey> listFlightRoute)
        {

            foreach(var itemRoute in listFlightRoute)
            {
                var insertJourneyflight = await _journeyRepository.InsertJourneyflightAsync(itemRoute.Origin, itemRoute.Destination, itemRoute.Price);


                foreach (var itemFlight in itemRoute.flight)
                {
                    
                    Transport transport = new Transport();
                    transport = itemFlight.transport;

                    var responseInserTranport =  await _journeyRepository.InsertTranportAsync(transport);
                    var lastTranportId= await _journeyRepository.GetLastSavedTransportAsync();
                    if (lastTranportId.SingleOrDefault().IdTransport != 0)
                    {
                        var responseFlight = await _journeyRepository.InsertFlightAsync(lastTranportId.SingleOrDefault().IdTransport, itemFlight.Origin, itemFlight.Destination, itemFlight.Price,Convert.ToInt32(insertJourneyflight.SingleOrDefault().IdFlight));
                    }

                }
                
            }
            return new Response<dynamic>() { success = true, error = false, result = null };
        }

        public async Task<Response<dynamic>> GetPersistConsultedFlights(dynamic? consultRegisteredFlights)
        {
            List<Journey> listFlightRoute = new List<Journey>();

            foreach(var itemJourney in consultRegisteredFlights)
            {
                var test = itemJourney.Origin;
                var listFlight = await _journeyRepository.GetListFlightsJourneyAsync(itemJourney.Origin, itemJourney.Destination);

                List<Flight> listOfOrderedFlights = new List<Flight>();
                foreach (var itemFlight in listFlight)
                {  

                    Flight flightOrderedOrigin = new Flight();
                    flightOrderedOrigin.Origin = itemFlight.Origin;
                    flightOrderedOrigin.Destination = itemFlight.Destination;
                    flightOrderedOrigin.Price = itemFlight.Price;
                    flightOrderedOrigin.transport = ConvertObjetToClass(await _journeyRepository.GetListFlightsTransportAsync(itemFlight.IdTransport));
                    listOfOrderedFlights.Add(flightOrderedOrigin);
                }


                Journey journey = new Journey();
                journey.Origin = itemJourney.Origin;
                journey.Destination = itemJourney.Destination;
                journey.flight = listOfOrderedFlights;
                journey.Price = itemJourney.Price;
                listFlightRoute.Add(journey);


            }

            return new Response<dynamic>() { success = true, error = false, result = listFlightRoute };
        }

        private Transport ConvertObjetToClass(dynamic transport)
        {
            Transport listTranport = new Transport();
            foreach(var itemTransport in transport)
            {
                listTranport.FlightCarrier = itemTransport.FlightCarrier;
                listTranport.FlightNumber = itemTransport.FlightNumber;

            }
            return listTranport;
        }
    }
}
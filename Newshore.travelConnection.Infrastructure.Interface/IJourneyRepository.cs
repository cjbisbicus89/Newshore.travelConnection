using Newshore.travelConnection.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Infrastructure.Interface
{
    public interface IJourneyRepository
    {
        Task<IEnumerable<dynamic>> GetListFlightsAsync();
        Task<IEnumerable<dynamic>> GetListFlightsSaveAsync(string origin, string destination);
        Task<bool> InsertTranportAsync(Transport transport);

        Task<IEnumerable<dynamic>> GetLastSavedTransportAsync();

        Task<IEnumerable<dynamic>> InsertFlightAsync(int idTransport, string @origin, string @destination, double @price, int idJourney);

        Task<IEnumerable<dynamic>> InsertJourneyflightAsync(string @origin, string @destination, double @price);
        Task<IEnumerable<dynamic>> GetListFlightsJourneyAsync(string origin, string destination);
        Task<IEnumerable<dynamic>> GetListFlightsTransportAsync(int idTranports);
    }
}

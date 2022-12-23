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
    }
}

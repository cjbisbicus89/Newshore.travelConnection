using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Domain.Interface
{
    public  interface IJourneyDomain
    {
        Task<Response<dynamic>> GetListFlightsAsync();
        Task<Response<dynamic>> GetJourneyByOriginAndDestination(string origin, string destination );
    }
}

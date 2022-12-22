using Newshore.travelConnection.Application.DTO;
using Newshore.travelConnection.Domain.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Application.Interface
{
    public interface IJourneyAplication
    {
        Task<Response<dynamic>> GetListFlightsAsync();

        Task<Response<dynamic>> GetJourneyByOriginAndDestination(string origin, string destination );
    }
}

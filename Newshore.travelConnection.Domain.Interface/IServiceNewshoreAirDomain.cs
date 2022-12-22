using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Newshore.travelConnection.Domain.Interface
{
    public interface IServiceNewshoreAirDomain
    {

        Task<string> GetServiceNewshore(Task<string>? urlApiNewshore);
        Task<List<Flight>> ConvertlistToObjet(Task<string>? responseApiNewshore);
        Task<string> GetUrlApiNewshore(IEnumerable<dynamic>? listServiceNewshore);

    }
}

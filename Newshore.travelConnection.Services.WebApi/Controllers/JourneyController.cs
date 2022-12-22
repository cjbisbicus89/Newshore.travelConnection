using Microsoft.AspNetCore.Mvc;
using Newshore.travelConnection.Application.DTO;
using Newshore.travelConnection.Application.Interface;
using System.Threading.Tasks;
using Newshore.travelConnection.Domain.Entity.Response;

namespace Newshore.travelConnection.Services.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JourneyController :  Controller
    {
        private readonly IJourneyAplication _journeylApplication;

        public JourneyController(IJourneyAplication journeylApplication)
        {
            _journeylApplication = journeylApplication;
        }

        #region "Métodos Sincronos"
        #endregion

        #region "Métodos Asincronos"
        [HttpGet]
        public async Task<Response<dynamic>> GetListFlightsAsync()
        {
            return await _journeylApplication.GetListFlightsAsync();
        }
        [HttpGet]
        public async Task<Response<dynamic>> GetJourneyByOriginAndDestination(string origin, string destination)
        {
            return await _journeylApplication.GetJourneyByOriginAndDestination(origin,destination);
        }
        #endregion
    }
}

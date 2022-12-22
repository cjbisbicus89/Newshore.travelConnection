using AutoMapper;
using Newshore.travelConnection.Application.Interface;
using Newshore.travelConnection.Domain.Entity.Response;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Transversal.Common;

namespace Newshore.travelConnection.Application.Main
{
    public class JourneyAplication:IJourneyAplication
    {
        #region global
        private readonly IJourneyDomain _journeyDomain;
        private readonly IMapper _mapper;
        private readonly IAppLogger<JourneyAplication> _logger;
        #endregion

        public JourneyAplication(IJourneyDomain journeyDomain, IMapper mapper, IAppLogger<JourneyAplication> logger)
        {
            _journeyDomain = journeyDomain;
            _mapper = mapper;
            _logger = logger;
        }

        #region Métodos Síncronos
        #endregion


        #region Métodos Asíncronos
        public async Task<Response<dynamic>> GetListFlightsAsync()
        {

            try
            {
                var response = await _journeyDomain.GetListFlightsAsync();
                response.message = "Consulta Exitosa";
                return response;
            }
            catch (Exception e)
            {
                return new Response<dynamic>() { success = false, error = true, message = "No se pudo realizar la consulta" };
                _logger.LogError(e.Message);
            }

        }
        public async Task<Response<dynamic>> GetJourneyByOriginAndDestination(string origin, string destination )
        {
            try
            {
                var response = await _journeyDomain.GetJourneyByOriginAndDestination(origin, destination);
                response.message = "Consulta Exitosa";
                return response;
            }
            catch (Exception e)
            {
                return new Response<dynamic>() { success = false, error = true, message = "No se pudo realizar la consulta" };
                _logger.LogError(e.Message);
            }
        }
        #endregion

    }
}
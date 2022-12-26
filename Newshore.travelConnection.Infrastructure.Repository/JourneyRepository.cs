using Dapper;
using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Domain.Entity.Response;
using Newshore.travelConnection.Infrastructure.Interface;
using Newshore.travelConnection.Transversal.Common;
using System.Data;


namespace Newshore.travelConnection.Infrastructure.Repository
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public JourneyRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<dynamic>> GetListFlightsAsync()
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "listConfiguration";

                var responseProcedure = await connection.QueryAsync<ServicesRestApi>(queryProcedure, commandType: CommandType.StoredProcedure);
                return responseProcedure;
            }
        }

        public async Task<IEnumerable<dynamic>> GetListFlightsSaveAsync(string origin, string destination)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "ConsultFlights";
                var parameters = new DynamicParameters();
                parameters.Add("origin", origin);
                parameters.Add("destination", destination);

                var responseProcedure = await connection.QueryAsync<Journey>(queryProcedure, param: parameters, commandType: CommandType.StoredProcedure);
                return responseProcedure;
            }
        }

        public async Task<bool> InsertTranportAsync(Transport transport)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "InsertTransport";
                var parameters = new DynamicParameters();
                parameters.Add("flightCarrier", transport.FlightCarrier);
                parameters.Add("flightNumber", transport.FlightNumber);

                var result = await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<IEnumerable<dynamic>> GetLastSavedTransportAsync()
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "LastSavedTransport";
                var responseProcedure = await connection.QueryAsync<TranportResponse>(queryProcedure, commandType: CommandType.StoredProcedure);
                return responseProcedure;
            }
        }

        public async Task<IEnumerable<dynamic>> InsertFlightAsync(int idTransport, string @origin, string @destination, double @price, int idJourney)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "InsertFlight";
                var parameters = new DynamicParameters();
                parameters.Add("id_transport", idTransport);
                parameters.Add("origin", @origin);
                parameters.Add("destination", @destination);
                parameters.Add("price", Convert.ToInt32(@price));
                parameters.Add("idJourney", idJourney);

                var responseProcedure = await connection.QueryAsync<FlightResponse>(queryProcedure, param: parameters, commandType: CommandType.StoredProcedure);

                return responseProcedure;
            }
        }

        public async Task<IEnumerable<dynamic>> InsertJourneyflightAsync(string @origin, string @destination, double @price)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "InsertJourneyflight";
                var parameters = new DynamicParameters();

                parameters.Add("origin", @origin);
                parameters.Add("destination", @destination);
                parameters.Add("price", Convert.ToInt32(@price));

                var responseProcedure = await connection.QueryAsync<FlightResponse>(query, param: parameters, commandType: CommandType.StoredProcedure);

                return responseProcedure;
            }
        }

        public async Task<IEnumerable<dynamic>> GetListFlightsJourneyAsync(string origin, string destination)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "ConsultJourneyFlights";
                var parameters = new DynamicParameters();
                parameters.Add("origin", origin);
                parameters.Add("destination", destination);

                var responseProcedure = await connection.QueryAsync<FlightResponse>(queryProcedure, param: parameters, commandType: CommandType.StoredProcedure);
                return responseProcedure;
            }
        }

        public async Task<IEnumerable<dynamic>> GetListFlightsTransportAsync(int idTranports)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var queryProcedure = "ConsultTransport";
                var parameters = new DynamicParameters();
                parameters.Add("id", idTranports);
             
                var responseProcedure = await connection.QueryAsync<Transport>(queryProcedure, param: parameters, commandType: CommandType.StoredProcedure);
                return responseProcedure;
            }
        }

    }
}
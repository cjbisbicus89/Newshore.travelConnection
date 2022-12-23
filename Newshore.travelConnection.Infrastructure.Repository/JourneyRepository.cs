using System;
using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Infrastructure.Interface;
using Newshore.travelConnection.Transversal.Common;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;


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



    }
}
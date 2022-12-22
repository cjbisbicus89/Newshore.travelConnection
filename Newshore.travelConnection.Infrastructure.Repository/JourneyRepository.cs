using System;
using Newshore.travelConnection.Domain.Entity;
using Newshore.travelConnection.Infrastructure.Interface;
using Newshore.travelConnection.Transversal.Common;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;


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

    }
}
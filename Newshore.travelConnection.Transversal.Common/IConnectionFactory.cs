using System.Data;


namespace Newshore.travelConnection.Transversal.Common
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}

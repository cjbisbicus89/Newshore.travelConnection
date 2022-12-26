using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Domain.Entity.Response
{
    public  class FlightResponse
    {
        public int? IdFlight { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }

        public int IdTransport { get; set;  }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Domain.Entity.ExternalResponseModels
{
    public  class RecruitingAPI
    {
        public string departureStation { get; set; }
        public string arrivalStation { get; set; }
        public string flightCarrier { get; set; }
        public string flightNumber { get; set; }
        public int price { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Domain.Entity
{
    public class ServicesRestApi
    {
        public int id { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public bool flg_active { get; set; }

    }
}

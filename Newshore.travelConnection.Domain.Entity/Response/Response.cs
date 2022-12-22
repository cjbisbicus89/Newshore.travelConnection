using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.travelConnection.Domain.Entity.Response
{
    public class Response<T>
    {
        public T result { get; set; }
        public bool success { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
    }
}

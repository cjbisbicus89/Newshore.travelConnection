namespace Newshore.travelConnection.Domain.Entity
{
    public class Journey
    {
        
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public List<Flight> flight { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SignalRStreamingJson.Models
{
    public class MockDataTable
    {
        public MockDataTable() { }

        [Key]
        public int? ID { get; set; }
        public string? AnimalName { get; set; }
        public string? LatinName { get; set; }
    }
}

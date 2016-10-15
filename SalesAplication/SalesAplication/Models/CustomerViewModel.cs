using System;

namespace SalesAplication.Models
{
  public class CustomerViewModel
    {

        public string Name { get; set; }
        public string Gender { get; set; }
        public int? City { get; set; }
        public int ?Region { get; set; }
        public DateTime? LastPurchase { get; set; }
        public DateTime ?LastPurchaseEnd { get; set; }
        public int? Classification { get; set; }
        public int? Selle { get; set; }
    }
}

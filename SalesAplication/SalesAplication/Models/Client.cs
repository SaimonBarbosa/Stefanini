using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace SalesAplication.Models
{
    public partial class Client
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> LastPurchase { get; set; }
        public int SellerId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public Nullable<int> ClassificationId { get; set; }
        public int ClientId { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }

        public virtual Classification Classification { get; set; }
        public virtual Region Region { get; set; }
        public virtual User User { get; set; }
    }
}
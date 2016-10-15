using System;
using System.Collections.Generic;

namespace SalesAplication.Models
{

    public partial class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }     
    }
}
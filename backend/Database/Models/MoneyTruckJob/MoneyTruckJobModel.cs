using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobModel
    {
        public int Id { get; set; }
        public int StartLocationId { get; set; }
        public int RouteId { get; set; }
        public int Reward { get; set; }
    }
}

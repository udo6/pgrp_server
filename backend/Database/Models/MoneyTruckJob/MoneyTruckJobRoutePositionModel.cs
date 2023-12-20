using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobRoutePositionModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int PositionId { get; set; }

        [NotMapped] public bool HasBeenPickedUp { get; set; } = false;
    }
}

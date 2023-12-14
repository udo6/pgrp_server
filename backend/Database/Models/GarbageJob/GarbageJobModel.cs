using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.GarbageJob
{
    public class GarbageJobModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int VehicleSpawnPositionId { get; set; }
        public int GarbageReturnPositionId { get; set; }
        public int Price { get; set; }

        [NotMapped] public int TruckCount { get; set; } = 0;
    }
}

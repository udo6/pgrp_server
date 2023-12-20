using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobRouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }

        [NotMapped] public bool InWork { get; set; }
        [NotMapped] public DateTime LastUsed { get; set; } = DateTime.Now.AddMinutes(-10);
    }
}

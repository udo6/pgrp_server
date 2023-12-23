using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.MoneyTruck
{
    public class MoneyTruckActiveRouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }

        public bool InWork { get; set; }
        public DateTime LastUsed { get; set; }
        public int PlayerId { get; set; }

        public MoneyTruckActiveRouteModel()
        {
            InWork = false;
            LastUsed = DateTime.Now.AddMinutes(-10);
            PlayerId = 0;
        }

        public MoneyTruckActiveRouteModel(int id, string name, int reward, bool inWork, DateTime lastUsed, int playerId)
        {
            Id = id;
            Name = name;
            Reward = reward;
            InWork = inWork;
            LastUsed = lastUsed;
            PlayerId = playerId;
        }
    }
}

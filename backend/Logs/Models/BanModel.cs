using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Models
{
	public class BanModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int AdminId { get; set; }
		public string Reason { get; set; }

		public BanModel()
		{
			Reason = string.Empty;
		}

		public BanModel(int accountId, int adminId, string reason)
		{
			AccountId = accountId;
			AdminId = adminId;
			Reason = reason;
		}
	}
}

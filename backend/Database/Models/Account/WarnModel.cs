using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Account
{
	public class WarnModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Reason { get; set; }
		public int AdminId { get; set; }
		public DateTime Date { get; set; }

		public WarnModel()
		{
			Reason = string.Empty;
		}

		public WarnModel(int accountId, string reason, int adminId, DateTime date)
		{
			AccountId = accountId;
			Reason = reason;
			AdminId = adminId;
			Date = date;
		}
	}
}

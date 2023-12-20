using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Account
{
	public class AdminHistoryModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Reason { get; set; }
		public int AdminId { get; set; }
		public string AdminName { get; set; }
		public DateTime Date { get; set; }
		public AdminHistoryType Type { get; set; }

		public AdminHistoryModel()
		{
			Reason = string.Empty;
			AdminName = string.Empty;
		}

		public AdminHistoryModel(int accountId, string reason, int adminId, string adminName, DateTime date, AdminHistoryType type)
		{
			AccountId = accountId;
			Reason = reason;
			AdminId = adminId;
			AdminName = adminName;
			Date = date;
			Type = type;
		}
	}
}

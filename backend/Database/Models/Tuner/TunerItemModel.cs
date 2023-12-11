using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Tuner
{
	public class TunerItemModel
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public int ModCategory { get; set; }
		public int ModValue { get; set; }

		public TunerItemModel()
		{
			Name = string.Empty;
		}

		public TunerItemModel(int categoryId, string name, int modCategory, int modValue)
		{
			CategoryId = categoryId;
			Name = name;
			ModCategory = modCategory;
			ModValue = modValue;
		}
	}
}

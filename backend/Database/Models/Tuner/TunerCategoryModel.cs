using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Tuner
{
	public class TunerCategoryModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TunerCategoryModel()
		{
			Name = string.Empty;
		}

		public TunerCategoryModel(string name)
		{
			Name = name;
		}
	}
}

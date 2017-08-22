using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.Models
{
	public class Planet
	{
		public string Biome { get; set; }

		public Dictionary<PlanetSize,Stats> Stats { get; set; }

		//public Stats Huge { get; set; }
		//public Stats Large { get; set; }
		//public Stats Medium { get; set; }
		//public Stats Small { get; set; }
		//public Stats Tiny { get; set; }
	}

	public enum PlanetSize
	{
		Tiny,
		Small,
		Medium,
		Large,
		Huge
	}

	public class Stats
	{
		public int MaxPop { get; set; }
		public string Food { get; set; }
	}
}
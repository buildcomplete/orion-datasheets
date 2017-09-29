using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class ShipHull
	{
		public string Name { get; set; }
		public double Strength { get; set; }
		public double Capacity { get; set; }
		public int Tier { get; internal set; }
	}
}

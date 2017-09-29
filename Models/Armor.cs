using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Armor
	{
		public string Name { get; set; }

		public double Resilience { get; set; }

		public double BaseCost { get; set; }

		public double CostHullFactor { get; set; }

		public double TacticalArmorMultiplier { get; set; }

		public double MissileArmorMultiplier { get; set; }
		public int Tier { get; internal set; }
	}
}

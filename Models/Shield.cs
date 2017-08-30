using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Shield
	{
		public string Name { get; set; }
		public double Absorption { get; set; }
		public double Multiplier { get; set; }
		public double RechargeRate { get; set; }
		public double Cooldown { get; set; }
		public double BaseSize { get; set; }
		public double SizeHullFactor { get; set; }
		public string AssetFilename { get; set; }
	}
}

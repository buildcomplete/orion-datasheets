using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
	public class ShieldRepository
	{
		public List<Shield> Shields = new List<Shield>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Data/Shield-data.csv");
			foreach (var item in data.Skip(1))
			{
				var tokens = item.Trim().Split(',');
				Shields.Add(new Shield
				{
					Name = tokens[0],
					Absorption = double.Parse(tokens[1]),
					Multiplier = double.Parse(tokens[2]),
					RechargeRate = double.Parse(tokens[3]),
					Cooldown = double.Parse(tokens[4]),
					BaseSize = double.Parse(tokens[5]),
					SizeHullFactor = double.Parse(tokens[6]),
					AssetFilename = tokens[7],
					Tier = int.Parse(tokens[8])
				});
			}
		}
	}
}

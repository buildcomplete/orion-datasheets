using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Models.Repository
{
	public class ArmorRepository
	{
		public List<Armor> Armors = new List<Armor>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Data/Armor-data.csv");
			foreach (var item in data.Skip(1))
			{
				var tokens = item.Trim().Split(',');
				Armors.Add(new Armor
				{
					Name = tokens[0],
					Resilience = double.Parse(tokens[1]),
					BaseCost = double.Parse(tokens[2]),
					CostHullFactor = double.Parse(tokens[3]),
					TacticalArmorMultiplier = double.Parse(tokens[4]),
					MissileArmorMultiplier = double.Parse(tokens[5]),
					Tier = int.Parse(tokens[6])
				});
			}
		}
	}
}

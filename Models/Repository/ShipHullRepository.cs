using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Repository
{
	public class ShipHullRepository
	{
		public List<ShipHull> Hulls = new List<ShipHull>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Data/ShipHull-data.csv");
			foreach (var item in data.Skip(1))
			{
				var tokens = item.Trim().Split(',');
				Hulls.Add(new ShipHull
				{
					Name = tokens[0],
					Strength = double.Parse(tokens[1]),
					Capacity = double.Parse(tokens[2]),
				});
			}
		}
	}
}

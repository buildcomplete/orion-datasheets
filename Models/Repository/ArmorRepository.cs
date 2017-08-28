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
		public List<Armor> Weapons = new List<Armor>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Data/Armor-data.csv");
			foreach (var item in data.Skip(1))
			{
				var tokens = item.Trim().Split(',');
				Weapons.Add(new Armor
				{
					Name = tokens[0],
					Rating = double.Parse(tokens[1])
				});
			}
		}
	}
}

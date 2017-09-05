using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Repository
{
	public class WeaponRepository
	{
		public List<Weapon> Weapons = new List<Weapon>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Data/Weapon-data.csv");
			foreach (var item in data.Skip(1))
			{
				var tokens = item.Trim().Split(',');
				Weapons.Add(new Weapon
				{
					Name = tokens[0],
					Cooldown = double.Parse(tokens[1]),
					Damage= double.Parse(tokens[2]),
					DamageProcs = double.Parse(tokens[3]),
					ArmorPenetration= double.Parse(tokens[4]),
					ShieldPiercing = tokens[5]=="1",
					Cost = double.Parse(tokens[6]),
					Size = double.Parse(tokens[7]),
					Type = (WeaponType)Enum.Parse(typeof(WeaponType),tokens[8])
				});
			}
		}
	}
}

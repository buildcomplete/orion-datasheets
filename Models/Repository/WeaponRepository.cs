using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Models.Repository
{
	public class WeaponRepository
	{
		public List<Weapon> Weapons = new List<Weapon>();

		public async Task Initialize()
		{
			var data = await CsvHelper.GetLines("ms-appx:///Models/Assets/Weapon-data.csv");
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
					Type = (WeaponType)Enum.Parse(typeof(WeaponType),tokens[6])
				});
			}
		}
	}
}

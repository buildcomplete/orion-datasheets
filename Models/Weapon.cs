using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Weapon
	{
		public string Name { get; set; }

		public double Cooldown { get; set; }
		public double Damage { get; set; }
		public double DamageProcs { get; set; } = 1;
		public double ArmorPenetration { get; set; }
		public bool ShieldPiercing { get; set; }
		public WeaponType Type { get; set; }

		public override string ToString()
		{
			return $"{Name} - Damage: ({Damage}x{DamageProcs}) / {Cooldown} - AP: {ArmorPenetration}, SP:{(ShieldPiercing?"YES":"NO")} - {Type}";
		}

		public double Dps(double vsArmor=0)
		{
			var actualArmor = Math.Max(0, vsArmor - ArmorPenetration);
			return (Math.Max(0,(Damage- actualArmor)) * DamageProcs) / Cooldown;
		}
	}

	public enum WeaponType
	{
		Energy,
		Missile,
		Torpedo
	}
}

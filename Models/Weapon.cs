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

		public double GetDamageMultiplier(double armorResilience)
		{
			// Formula from user WhatIsSol from steamcommunity thread
			// Weapon.Damage * Max((Weapon.Penetration / Target.Resilience), 0.75)
			// http://steamcommunity.com/app/298050/discussions/0/154642447922416143/
			return armorResilience >= 1.0
				? Math.Max(ArmorPenetration / armorResilience, 0.75)
				: 1;
		}

		/// <summary>
		/// Weapon Damage against armor with specified resilience
		/// If resilience is less than one, the raw weapon damage is returned
		/// </summary>
		/// <param name="armorResilience"></param>
		/// <returns>Weapon Damage against armor with specified resilience</returns>
		public double DamageVs(double armorResilience)
		{
			return Damage * GetDamageMultiplier(armorResilience) * DamageProcs;
		}

		public double DpsVs(double armorResilience = 0)
		{
			return DamageVs(armorResilience) / Cooldown;
		}
	}

	public enum WeaponType
	{
		Energy,
		Missile,
		Torpedo
	}
}

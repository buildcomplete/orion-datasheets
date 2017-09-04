using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Weapon : SimulatedObject
	{
		#region Base properties
		public string Name { get; set; }
		public double Cooldown { get; set; }
		public double Damage { get; set; }
		public double DamageProcs { get; set; } = 1;
		public double ArmorPenetration { get; set; }
		public bool ShieldPiercing { get; set; }
		public WeaponType Type { get; set; }
		#endregion

		// Creates a copy of the current weapon with all it's basic properties.
		public Weapon BaseCopy()
			=> new Weapon
			{
				Name = Name,
				Cooldown = Cooldown,
				Damage = Damage,
				DamageProcs = DamageProcs,
				ArmorPenetration = ArmorPenetration,
				ShieldPiercing = ShieldPiercing,
				Type = Type
			};


		public ShipConfiguration Target { get; set; }

		public override string ToString()
			=> $"{Name} - Damage: ({Damage}x{DamageProcs}) / {Cooldown} - AP: {ArmorPenetration}, SP:{(ShieldPiercing ? "YES" : "NO")} - {Type}";


		// Gets damage multiplier.
		// Formula from user WhatIsSol from steamcommunity thread
		// Weapon.Damage * Max((Weapon.Penetration / Target.Resilience), 0.75)
		// http://steamcommunity.com/app/298050/discussions/0/154642447922416143/
		public double GetDamageMultiplier(double armorResilience)
			=> armorResilience >= 1.0
				? Math.Max(ArmorPenetration / armorResilience, 0.75)
				: 1;

		public double ModDamage { get => Damage; }

		/// <summary>
		/// Weapon Damage against armor with specified resilience
		/// If resilience is less than one, the raw weapon damage is returned
		/// </summary>
		/// <param name="resilience"></param>
		/// <returns>Weapon Damage against armor with specified resilience</returns>
		public double DamageVsArmor(double resilience)
			=> ModDamage * GetDamageMultiplier(resilience) * DamageProcs;
		
		public double DpsVs(double armorResilience = 0)
			=> DamageVsArmor(armorResilience) / Cooldown;

		public double DamageVsShield(double absorption)
			=> Math.Max(0, (ModDamage - absorption)) * DamageProcs;
		
		double _heat = -1;
		public void Tick(double dt)
		{
			// Cooldown.
			if (_heat > 0)
				_heat -= dt;

			if (_heat <= 0 && Target != null)
			{
				Target.TakeHit(this);

				// Apply heat
				_heat = Cooldown;
			}
		}
	}

	public enum WeaponType
	{
		Energy,
		Missile,
		Torpedo
	}
}

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
		public double Cost { get; set; }
		public double Size { get; set; }
		public WeaponType Type { get; set; }
		public WeaponModifiers PossibleModifiers { get; set; }
		public WeaponModifiers EnabledModifiers { get; set; }

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
				Type = Type,
				PossibleModifiers = PossibleModifiers,
				EnabledModifiers = EnabledModifiers
			};


		public ShipConfiguration Target { get; set; }

		public override string ToString()
			=> $"{Name} - Damage: ({ModDamage}x{ModDamageProcs}) / {ModCooldown} - AP: {ModArmorPenetration}, SP:{(ShieldPiercing ? "YES" : "NO")} - {Type}";

		#region Properties with modifiers
		public double ModDamage
		{
			get => Damage * (1
				+ (EnabledModifiers.HasFlag(WeaponModifiers.heavy_mount) ? 1 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.point_defense) ? -0.5 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.mirv) ? -0.5 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.overloaded) ? 1 : 0)
				);
		}

		public double ModShieldDamage
		{
			get => ModDamage * (1
				+ (EnabledModifiers.HasFlag(WeaponModifiers.enveloping) ? 0.5 : 0));
		}


		public double ModCooldown
		{
			get => Cooldown * (1
				+ (EnabledModifiers.HasFlag(WeaponModifiers.auto_fire) ? -0.50 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.point_defense) ? -0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.heavy_mount) ? 0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.overloaded) ? 0.25 : 0)
				);
		}


		public double ModSize
		{
			get => Size * (1
				+ (EnabledModifiers.HasFlag(WeaponModifiers.point_defense) ? -0.66 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.heavy_mount) ? 0.50 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.enveloping) ? 0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.auto_fire) ? 0.50 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.continuous_fire) ? 0.50 : 0)
				
				+ (EnabledModifiers.HasFlag(WeaponModifiers.heavy_armor) ? 0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.eccm) ? 0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.fast) ? 0.25 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.mirv) ? 0.50 : 0)

				+ (EnabledModifiers.HasFlag(WeaponModifiers.overloaded) ? 0.50 : 0)
				+ (EnabledModifiers.HasFlag(WeaponModifiers.semi_guided) ? 0.25 : 0)
				);
		}

		public double ModArmorPenetration
		{
			get => ArmorPenetration * (1
				+ (EnabledModifiers.HasFlag(WeaponModifiers.continuous_fire) ? 0.5 : 0));
		}

		public double ModDamageProcs
		{
			get => EnabledModifiers.HasFlag(WeaponModifiers.mirv) 
				? 4 
				: DamageProcs;
		}
		public int Tier { get; internal set; }
		#endregion

		// Gets damage multiplier.
		// Formula from user WhatIsSol from steamcommunity thread
		// Weapon.Damage * Max((Weapon.Penetration / Target.Resilience), 0.75)
		// http://steamcommunity.com/app/298050/discussions/0/154642447922416143/
		public double GetDamageMultiplier(double armorResilience)
			=> armorResilience >= 1.0
				? Math.Max(ModArmorPenetration / armorResilience, 0.75)
				: 1;

		/// <summary>
		/// Weapon Damage against armor with specified resilience
		/// If resilience is less than one, the raw weapon damage is returned
		/// </summary>
		/// <param name="resilience"></param>
		/// <returns>Weapon Damage against armor with specified resilience</returns>
		public double DamageVsArmor(double resilience)
			=> ModDamage * GetDamageMultiplier(resilience) * ModDamageProcs;

		public double DpsVs(double armorResilience = 0)
			=> DamageVsArmor(armorResilience) / ModCooldown;

		public double DamageVsShield(double absorption)
			=> Math.Max(0, (ModShieldDamage - absorption)) * ModDamageProcs;

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
				_heat = ModCooldown;
			}
		}
	}

	public enum WeaponType
	{
		Energy,
		Missile,
		Torpedo
	}

	[Flags]
	public enum WeaponModifiers
	{
		None = 0,
		point_defense = 0x001,
		continuous_fire = 0x002,
		auto_fire = 0x004,
		heavy_mount = 0x008,
		enveloping = 0x010,
		heavy_armor = 0x020,
		eccm = 0x040,
		fast = 0x080,
		mirv = 0x100,
		overloaded = 0x200,
		semi_guided = 0x400,
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class ShipConfiguration : SimulatedObject
	{
		public ShipConfiguration(ShipHull hull, Armor armor, Shield shield)
		{
			Hull = hull;
			Armor = armor;
			Shield = shield;
		}

		public void Initialize()
		{
			ShieldPoints = ShieldMaxHitPoints;
			HullPoints = ShipHullStrength;
		}

		public ShipHull Hull;
		public Shield Shield;
		public Armor Armor;

		public double ShipHullStrength
		{
			get =>
				(ModReinforcedHull
					? Hull.Strength * 0.5
					: 0)
				+ (Hull.Strength * Armor.TacticalArmorMultiplier);
		}

		public double ShieldMaxHitPoints
		{
			get => IsDestroyed
				? 0 :
				Hull.Strength * Shield.Multiplier;
		}

		public double ArmorResilience
		{
			get => ModHeavyArmor
				? Armor.Resilience * 2
				: Armor.Resilience;
		}

		// The current amount of shield strength
		public double ShieldPoints;
		public double HullPoints;
		private double _shieldHeat = 0;

		public bool IsDestroyed;

		public bool ModReinforcedHull;
		public bool ModHeavyArmor;


		internal void TakeHit(Weapon w)
		{
			if (IsDestroyed)
				return;

			if (ShieldPoints > 0
				&& false == w.ShieldPiercing)
			{
				// The damage against the Shield unaffected by the armor rating.
				ShieldPoints -= w.DamageVsShield(Shield.Absorption);

				if (ShieldPoints < 0)
				{
					double remainingDamage = -ShieldPoints;
					ShieldPoints = 0;

					// apply remaining damage without shield damage boost for example from enveloping
					double pctDamageRemaining = remainingDamage / w.DamageVsShield(Shield.Absorption);
					HullPoints -= pctDamageRemaining * w.DamageVsArmor(ArmorResilience);
				}

				// Tell the shield that it need some time before being able to charge...
				_shieldHeat = Shield.Cooldown;

			}
			// No shield, apply full damage directly to hull
			else
			{
				HullPoints -= w.DamageVsArmor(ArmorResilience);
			}

			if (HullPoints <= 0 && IsDestroyed == false)
			{
				HullPoints = 0;
				IsDestroyed = true;
				ShieldPoints = 0;
			}
		}

		public void Tick(double dt)
		{
			if (_shieldHeat > 0)
				_shieldHeat -= dt;

			if (_shieldHeat <= 0 && ShieldPoints < ShieldMaxHitPoints)
			{
				// Whats the recharge rate???
				double carge_second = (Shield.RechargeRate / 10.0) * ShieldMaxHitPoints;

				ShieldPoints = Math.Min(
					ShieldMaxHitPoints,
					ShieldPoints + carge_second * dt);
			}
		}
	}
}

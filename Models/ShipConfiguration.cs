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
				// Here comes some guessing...
				// The damage against the Shield, 
				// is probably unaffected by the armor rating.
				ShieldPoints -= w.DamageVsShield(Shield.Absorption);

				double remainingDamage = 0;
				if (ShieldPoints < 0)
				{
					remainingDamage = -ShieldPoints;
					ShieldPoints = 0;
				}

				// Tell the shield that it need some time before being able to charge...
				// And what is the size of this??? 
				// for now I multiplied with 2 to make sure it doesn't recharge faster than weapons...
				_shieldHeat = Shield.Cooldown;

				// What to do with the wasted damage??
				// As a guess, I will apply the wasted damage to the hull.
				// It might be more correct to apply each proc individually,
				// But how is it done in the game???
				HullPoints -= remainingDamage * w.GetDamageMultiplier(
					 ArmorResilience);
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

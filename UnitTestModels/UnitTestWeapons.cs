
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Repository;

namespace UnitTestModels
{
	[TestClass]
	public class UnitTestWeapons
	{
		[TestMethod]
		public void TestCalculateDPSLaserCannon()
		{
			Weapon laserCannon = new Weapon
			{
				Name = "Laser Cannon",
				Cooldown = 6,
				Damage = 5,
				DamageProcs = 1,
				ArmorPenetration = 0,
				ShieldPiercing = false,
				Type = WeaponType.Energy
			};

			double ArmorRatingNoArmor = 0;
			Assert.AreEqual(0.83d, laserCannon.Dps(ArmorRatingNoArmor), 0.005);

			// No damage vs titanium armor with standard laser canon
			double ArmorRatingTitanium = 5;
			Assert.AreEqual(0, laserCannon.Dps(ArmorRatingTitanium), 0.005);

			// And ofcourse, no damage vs tritanium armor with standard laser canon
			double ArmorRatingTritanium = 10;
			Assert.AreEqual(0, laserCannon.Dps(ArmorRatingTritanium), 0.005);
		}

		[TestMethod]
		public void TestCalculateDPSNeutronBlaster()
		{
			Weapon neutronBlaster = new Weapon
			{
				Name = "Neutron Blaster",
				Cooldown = 7,
				Damage = 9,
				DamageProcs = 1,
				ArmorPenetration = 5,
				ShieldPiercing = false,
				Type = WeaponType.Energy
			};

			double ArmorRatingNoArmor = 0;
			Assert.AreEqual(1.29d, neutronBlaster.Dps(ArmorRatingNoArmor), 0.005);

			// full damage vs titanium armor with neutron blaster
			double ArmorRatingTitanium = 5;
			Assert.AreEqual(1.29, neutronBlaster.Dps(ArmorRatingTitanium), 0.005);

			// reduced damage with tritanium armor
			double ArmorRatingTritanium = 10;
			Assert.AreEqual(0.57f, neutronBlaster.Dps(ArmorRatingTritanium), 0.005);
		}

		[TestMethod]
		public void TestCalculateDPSMassDriverFromRepository()
		{
			var repo = new WeaponRepository();
			repo.Initialize().Wait();
			Weapon massDriver = repo.Weapons
				.Find(X => X.Name == "Mass Driver");

			Assert.IsNotNull(massDriver);

			double ArmorRatingNoArmor = 0;
			Assert.AreEqual(1.50d, massDriver.Dps(ArmorRatingNoArmor), 0.005);

			// full damage vs titanium armor with neutron blaster
			double ArmorRatingTitanium = 5;
			Assert.AreEqual(1.50, massDriver.Dps(ArmorRatingTitanium), 0.005);

			// reduced damage with neutronium armor
			double ArmorRatingNeutronium = 15;
			Assert.AreEqual(0.88f, massDriver.Dps(ArmorRatingNeutronium), 0.005);
		}
	}
}

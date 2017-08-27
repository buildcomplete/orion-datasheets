
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Repository;
using System.Threading.Tasks;

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

			double ArmorResilianceNoArmor = 0;
			Assert.AreEqual(0.83d, laserCannon.DpsVs(ArmorResilianceNoArmor), 0.0051);

			double ArmorResilianceTitanium = 5;
			Assert.AreEqual(0.63, laserCannon.DpsVs(ArmorResilianceTitanium), 0.0051);

			double ArmorResilianceTritanium = 10;
			Assert.AreEqual(0.63, laserCannon.DpsVs(ArmorResilianceTritanium), 0.0051);
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

			double ArmorResilianceNoArmor = 0;
			Assert.AreEqual(1.29d, neutronBlaster.DpsVs(ArmorResilianceNoArmor), 0.0051);

			// full damage vs titanium armor with neutron blaster
			double ArmorResilianceTitanium = 5;
			Assert.AreEqual(1.29, neutronBlaster.DpsVs(ArmorResilianceTitanium), 0.0051);

			// reduced damage with tritanium armor
			double ArmorResilianceTritanium = 10;
			Assert.AreEqual(0.96f, neutronBlaster.DpsVs(ArmorResilianceTritanium), 0.0051);
		}

		[TestMethod]
		public async Task TestCalculateDPSMassDriverFromRepository()
		{
			var repo = new WeaponRepository();
			await repo.Initialize();
			Weapon massDriver = repo.Weapons
				.Find(X => X.Name == "Mass Driver");

			Assert.IsNotNull(massDriver);

			double ArmorResilianceNoArmor = 0;
			Assert.AreEqual(1.50d, massDriver.DpsVs(ArmorResilianceNoArmor), 0.0051);

			// Increased! damage vs titanium armor with mass driver
			double ArmorResilianceTitanium = 5;
			Assert.AreEqual(3.00, massDriver.DpsVs(ArmorResilianceTitanium), 0.0051);

			// reduced damage with mass driver
			double ArmorResilianceNeutronium = 15;
			Assert.AreEqual(1.13f, massDriver.DpsVs(ArmorResilianceNeutronium), 0.0051);
		}
	}
}

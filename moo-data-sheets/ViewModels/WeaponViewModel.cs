using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class WeaponViewModel : BindableBase
	{
		private Weapon _weapon;
		public WeaponViewModel(Weapon weapon)
		{
			_weapon = weapon;
		}

		public string Name { get => _weapon.Name.ToUpper(); }
		public string Cooldown { get => _weapon.Cooldown.ToString("0.00"); }
		public string Damage { get => _weapon.Damage.ToString("0.00"); }
		public string DamageProcs { get => _weapon.DamageProcs.ToString("0.00"); }
		public string ArmorPenetration { get => _weapon.ArmorPenetration.ToString("0.00"); }
		public string ShieldPiercing { get => _weapon.ShieldPiercing ? "Yes": "No"; }

		public double DamageVsArmor(double armorResilience)
		{
			return _weapon.DamageVs(armorResilience);
		}

		public double DpsVsArmor(double armorResilience)
		{
			return _weapon.DpsVs(armorResilience);
		}

		public string ImageFile
		{
			get
			{
				if (_weapon != null)
				{
					Dictionary<string, string> strangeNameMap = new Dictionary<string, string>
					{
						{"Anti matter Torpedo", "antimatter_torpedoes" },
						{"Graviton Cannon", "graviton_beam" },
						{"Ion Pulse Beam", "ion_pulse_cannon" },
						{"Plasma Beam", "plasma_cannon" },
						{"Proton Torpedo", "proton_torpedoes" },
						{"Plasma Torpedo", "plasma_torpedoes" },
					};

					var mappedName = strangeNameMap.ContainsKey(_weapon.Name)
						? strangeNameMap[_weapon.Name]
						: _weapon.Name;

					return $"/Assets/ship_modules/{mappedName.Replace(' ', '_').ToLower()}.png";
				}
				return null;
			}
		}

		public override string ToString()
		{
			return $"{_weapon.Name}";
		}
	}
}

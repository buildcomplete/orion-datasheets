using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace moo_data_sheets.ViewModels
{
	public class WeaponViewModel : BindableBase
	{
		private Weapon _weapon;
		public WeaponViewModel(Weapon weapon)
		{
			_weapon = weapon;
		}

		public Weapon Model { get => _weapon; }

		public string Name { get => _weapon.Name.ToUpper(); }
		public string Cooldown { get => _weapon.Cooldown.ToString("0.00"); }
		public string DamageText { get =>
			_weapon.DamageProcs > 1
				? $"{_weapon.Damage} x {_weapon.DamageProcs} ({_weapon.Damage * _weapon.DamageProcs})"
				: $"{_weapon.Damage}";
		}
		public string ArmorPenetration { get => _weapon.ArmorPenetration.ToString("0.00"); }
		public string ShieldPiercingText { get => _weapon.ShieldPiercing ? "Yes": "No"; }
		public Visibility ShieldPiercingVis { get => _weapon.ShieldPiercing ? Visibility.Visible : Visibility.Collapsed; }
		public double DamageVsArmor(double armorResilience)
		{
			return _weapon.DamageVsArmor(armorResilience);
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

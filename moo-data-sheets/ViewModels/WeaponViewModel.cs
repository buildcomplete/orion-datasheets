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
			PossibleModifiers = Enum.GetNames(typeof(WeaponModifiers))
				.Select(X => (WeaponModifiers)Enum.Parse(typeof(WeaponModifiers), X))
				.Where(X => X != WeaponModifiers.None)
				.Where(X => _weapon.PossibleModifiers.HasFlag(X))
				.Select(X => new WeaponModifierViewModel { Modifier = X, WeaponVM = this })
				.ToArray();
		}

		public Weapon Model { get => _weapon; }

		public string Name { get => _weapon.Name.ToUpper(); }
		public int Tier { get => _weapon.Tier; }

		public string Cooldown { get => _weapon.ModCooldown.ToString("0.00"); }
		public string Size { get => _weapon.ModSize.ToString("0.00"); }

		public string DamageText { get =>
			_weapon.ModDamageProcs > 1
				? $"{_weapon.ModDamage} x {_weapon.ModDamageProcs} ({_weapon.ModDamage * _weapon.ModDamageProcs})"
				: $"{_weapon.ModDamage}";
		}
		public string ArmorPenetration { get => _weapon.ModArmorPenetration.ToString("0.00"); }
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

		public WeaponModifierViewModel[] PossibleModifiers
		{
			get ;set;
		}

		private WeaponModifiers _enabledModifiers;
		public WeaponModifiers EnabledModifiers
		{
			get => _weapon.EnabledModifiers;
			set
			{
				if (_weapon.EnabledModifiers != value)
				{
					_weapon.EnabledModifiers = value;
					RaisePropertyChanged();
					RaisePropertyChanged(nameof(Cooldown));
					RaisePropertyChanged(nameof(DamageText));
					RaisePropertyChanged(nameof(DamageVsArmor));
					RaisePropertyChanged(nameof(Size));
					RaisePropertyChanged(nameof(ArmorPenetration));
				}
			}
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
						{"Pirate Missile", "nuclear_missile" },
						
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

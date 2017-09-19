using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class WeaponModifierViewModel : BindableBase
	{
		public WeaponViewModel WeaponVM {get;set;}

		public WeaponModifiers Modifier { get; set; }

		public bool IsSet
		{
			get => WeaponVM.EnabledModifiers.HasFlag(Modifier);
			set
			{
				if (value != IsSet)
				{
					if (value)
						WeaponVM.EnabledModifiers |= Modifier;
					else
						WeaponVM.EnabledModifiers &= ~Modifier;
					RaisePropertyChanged(nameof(ImageFile));
				}
			}
		}

		public string ImageFile { get => IsSet
				? $"/Assets/weapon_modifiers/{Modifier}-selected.png"
				: $"/Assets/weapon_modifiers/{Modifier}.png"; }

		public override string ToString()
		{
			return Modifier.ToString() + ":" + IsSet.ToString();
		}
	}
}

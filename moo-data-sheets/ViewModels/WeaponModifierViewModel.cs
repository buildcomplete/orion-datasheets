using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class WeaponModifierViewModel
	{
		public WeaponViewModel WeaponVM {get;set;}

		public WeaponModifiers Modifier { get; set; }

		public bool IsSet
		{
			get => WeaponVM.EnabledModifiers.HasFlag(Modifier);
			set {
				if (value)
					WeaponVM.EnabledModifiers |= Modifier;
				else
					WeaponVM.EnabledModifiers &= ~Modifier;
			}
		}

		public string ImageFile { get => $"/Assets/weapon_modifiers/{Modifier}.png"; }

		public override string ToString()
		{
			return Modifier.ToString() + ":" + IsSet.ToString();
		}
	}
}

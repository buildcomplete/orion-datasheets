using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class WeaponViewModel
	{
		private Weapon _weapon;
		public WeaponViewModel(Weapon weapon)
		{
			_weapon = weapon;
		}

		public override string ToString()
		{
			return $"{_weapon.Name}";
		}
	}
}

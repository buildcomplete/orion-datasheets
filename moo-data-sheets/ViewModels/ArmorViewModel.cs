using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class ArmorViewModel : BindableBase
	{
		private Armor _armor;
		public ArmorViewModel(Armor armor)
		{
			_armor = armor;
		}

		public string Name { get => _armor.Name; }
		public double Rating { get => _armor.Rating; }

		public override string ToString()
		{
			return $"{Name} : {Rating}";
		}

	}
}

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

		public Armor Model { get => _armor; }

		public string Name { get => _armor.Name; }

		public double Resilience { get => _armor.Resilience; }

		public double TacticalArmorMultiplier { get => _armor.TacticalArmorMultiplier; }

		public override string ToString()
		{
			return $"{Name} : {Resilience}";
		}

	}
}

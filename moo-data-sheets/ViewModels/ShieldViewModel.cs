using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class ShieldViewModel : BindableBase
	{
		private Shield _shield;
		public ShieldViewModel(Shield shield)
		{
			_shield = shield;
		}

		public string Name { get => _shield.Name; }
		public double Absortion { get => _shield.Absorption; }
		public double Multiplier { get => _shield.Multiplier; }

		public override string ToString()
		{
			return $"{Name} : {Absortion}";
		}

	}
}

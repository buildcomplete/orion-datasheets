using Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moo_data_sheets.ViewModels
{
	public class ShipHullViewModel : BindableBase
	{
		private ShipHull _hull;
		public ShipHullViewModel(ShipHull hull)
		{
			_hull = hull;
		}

		public string Name { get => _hull.Name; }
		public double Strength { get => _hull.Strength; }

		public override string ToString()
		{
			return $"{Name} : {Strength}";
		}

	}
}

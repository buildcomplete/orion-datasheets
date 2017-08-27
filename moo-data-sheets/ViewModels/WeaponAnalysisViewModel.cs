using Models;
using Models.Repository;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace moo_data_sheets.ViewModels
{
	public class WeaponAnalysisViewModel : BindableBase
	{
		public ObservableCollection<WeaponViewModel> Weapons = new ObservableCollection<WeaponViewModel>();

		private WeaponViewModel _selectedWeapon;
		public WeaponViewModel SelectedWeapon
		{
			get => _selectedWeapon;
			set 
			{
				SetProperty(ref _selectedWeapon, value);
			}
		}
		
		public async Task Initialize()
		{
			var repo = new WeaponRepository();
			await repo.Initialize();
			foreach (var item in repo.Weapons)
			{
				Weapons.Add(new WeaponViewModel(item));
			}

			SelectedWeapon = Weapons[0];

			//PropertyChanged += (s, e) =>
			//{
			//	if (e.PropertyName == "SelectedWeapon")
			//	{
			//		SelectedWeapon.PublishAll();
			//	}
			//};

		}
	}
}

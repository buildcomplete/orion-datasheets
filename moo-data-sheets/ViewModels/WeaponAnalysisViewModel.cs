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
using Windows.UI.Xaml;
using Windows.UI.Core;

namespace moo_data_sheets.ViewModels
{
	public class WeaponAnalysisViewModel : BindableBase
	{
		public ObservableCollection<WeaponViewModel> Weapons = new ObservableCollection<WeaponViewModel>();

		public ObservableCollection<ArmorViewModel> ArmorTypes = new ObservableCollection<ArmorViewModel>();

		public WeaponAnalysisViewModel()
		{
			InitializeTask = Initialize();

			PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(SelectedWeapon)
					|| e.PropertyName == nameof(SelectedArmor))
				{
					RaisePropertyChanged(nameof(DamageVsArmor));
					RaisePropertyChanged(nameof(DpsVsArmor));
				}
			};

			InitializeTask.ContinueWith((T) =>
			{
				LoadErrorMessage = T.Exception?.Message;
			});
			ArmorTypes.Add(new ArmorViewModel(new Armor { Name = "No Armor (Raw DPS)", Rating = 0 }));
			ArmorTypes.Add(new ArmorViewModel(new Armor { Name = "Titanium", Rating = 5 }));
			ArmorTypes.Add(new ArmorViewModel(new Armor { Name = "Tritanium", Rating = 10 }));
			SelectedArmor = ArmorTypes[0];

		}

		private WeaponViewModel _selectedWeapon;
		public WeaponViewModel SelectedWeapon
		{
			get => _selectedWeapon;
			set => SetProperty(ref _selectedWeapon, value);
		}

		private ArmorViewModel _selectedArmor;
		public ArmorViewModel SelectedArmor
		{
			get => _selectedArmor;
			set => SetProperty(ref _selectedArmor, value);
		}

		private bool _loadCompleted = false;
		public bool LoadCompleted
		{
			get => _loadCompleted;
			set => SetProperty(ref _loadCompleted, value);
		}

		private string _loadErrorMessage;
		public string LoadErrorMessage
		{
			get => _loadErrorMessage;
			set => SetProperty(ref _loadErrorMessage, value);
		}

		public string DamageVsArmor { get => SelectedWeapon.DamageVsArmor(SelectedArmor.Rating).ToString("0.00"); }

		public string DpsVsArmor { get => SelectedWeapon.DpsVsArmor(SelectedArmor.Rating).ToString("0.00"); }


		private Task InitializeTask { get; set; }
		private async Task Initialize()
		{
			var repo = new WeaponRepository();
			await repo.Initialize();
			foreach (var item in repo.Weapons)
			{
				Weapons.Add(new WeaponViewModel(item));
				if (Weapons.Count == 1)
					SelectedWeapon = Weapons[0];
			}
			LoadCompleted = true;
		}
	}
}

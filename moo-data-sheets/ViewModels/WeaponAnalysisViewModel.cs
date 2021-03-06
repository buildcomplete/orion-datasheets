﻿using Models;
using Models.Repository;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Uwp;
using LiveCharts.Defaults;
using System.Linq;
using System.Collections.Generic;
using Windows.UI;

namespace moo_data_sheets.ViewModels
{
	public class WeaponAnalysisViewModel : BindableBase
	{
		public ObservableCollection<WeaponViewModel> Weapons = new ObservableCollection<WeaponViewModel>();
		public ObservableCollection<ArmorViewModel> ArmorTypes = new ObservableCollection<ArmorViewModel>();
		public ObservableCollection<ShieldViewModel> ShieldTypes = new ObservableCollection<ShieldViewModel>();
		public ObservableCollection<ShipHullViewModel> ShipHulls = new ObservableCollection<ShipHullViewModel>();

		public SeriesCollection SeriesCollection
		{
			get
			{
				if (SelectedWeapon == null
					||SelectedHull == null
					|| SelectedShield == null
					|| SelectedArmor == null)
					return null;

				ShipConfiguration shipConfig = new ShipConfiguration(
					SelectedHull.Model,
					SelectedArmor.Model,
					SelectedShield.Model);

				shipConfig.ModHeavyArmor = ModHeavyArmor;
				shipConfig.ModReinforcedHull = ModReinforcedHull;
				shipConfig.Initialize();

				SimulatedCollection s = new SimulatedCollection();
				for (int i=0;i<WeaponCount;++i)
				{
					Weapon w = SelectedWeapon.Model.BaseCopy();
					s.Add(w);
					w.Target = shipConfig;
				}

				s.Add(shipConfig);

				double hullStrengthPrevious = -1, shieldStrengthPrevious = -1;
				var hullValue = new ChartValues<ObservablePoint>();
				var shieldValue = new ChartValues<ObservablePoint>();
				double maxT = 200;
				double dt = 1;
				for (double t = 0; t < maxT; t+=dt)
				{
					if (shipConfig.HullPoints != hullStrengthPrevious)
						hullValue.Add(new ObservablePoint(t, shipConfig.HullPoints));

					if (shipConfig.ShieldPoints != shieldStrengthPrevious)
						shieldValue.Add(new ObservablePoint(t, shipConfig.ShieldPoints));

					hullStrengthPrevious = shipConfig.HullPoints;
					shieldStrengthPrevious = shipConfig.ShieldPoints;

					s.Tick(dt);
				}
				hullValue.Add(new ObservablePoint(maxT, shipConfig.HullPoints));
				shieldValue.Add(new ObservablePoint(maxT, shipConfig.ShieldPoints));


				return new SeriesCollection
				{
					 new StepLineSeries
					 {
						 Values = hullValue,
					 },
					 new StepLineSeries
					 {
						 Values = shieldValue,
					 },
				};
			}
		}

		public WeaponAnalysisViewModel()
		{
			PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(SelectedWeapon)
					|| e.PropertyName == nameof(SelectedArmor)
					|| e.PropertyName == nameof(SelectedShield)
					|| e.PropertyName == nameof(SelectedHull)
					|| e.PropertyName == nameof(WeaponCount)
					|| e.PropertyName == nameof(ModHeavyArmor)
					|| e.PropertyName == nameof(ModReinforcedHull))
				{
					RaiseCalculatedWeaponProptiesChanged();
				}
			};

			InitializeTask = Initialize();
			InitializeTask.ContinueWith((T) =>
			{
				LoadErrorMessage = T.Exception?.Message;
			});
		}

		private void RaiseCalculatedWeaponProptiesChanged()
		{
			RaisePropertyChanged(nameof(DamageVsArmor));
			RaisePropertyChanged(nameof(DpsVsArmor));
			RaisePropertyChanged(nameof(SeriesCollection));
			RaisePropertyChanged(nameof(SelectedWeaponTotalSpace));
			RaisePropertyChanged(nameof(MultiplierVsArmor));
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

		private ShieldViewModel _selectedShield;
		public ShieldViewModel SelectedShield
		{
			get => _selectedShield;
			set => SetProperty(ref _selectedShield, value);
		}

		private bool _ModHeavyArmor;
		public bool ModHeavyArmor
		{
			get => _ModHeavyArmor;
			set => SetProperty(ref _ModHeavyArmor, value);
		}

		private bool _ModReinforcedHull;
		public bool ModReinforcedHull
		{
			get => _ModReinforcedHull;
			set => SetProperty(ref _ModReinforcedHull, value);
		}

		public string SelectedWeaponTotalSpace { get =>
				SelectedWeapon != null
					? (SelectedWeapon.Model.ModSize * WeaponCount).ToString("0.0")
				: "-";
		}

		public IEnumerable<int> WeaponCountRange { get => Enumerable.Range(1, 200); }

		private int _weaponCount = 1;
		public int WeaponCount
		{
			get => _weaponCount;
			set => SetProperty(ref _weaponCount, value);
		}
		private ShipHullViewModel _selectedHull;
		public ShipHullViewModel SelectedHull
		{
			get => _selectedHull;
			set => SetProperty(ref _selectedHull, value);
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

		private double ModArmorResilience() => 
			SelectedArmor != null 
			? SelectedArmor.Resilience 
				* (ModHeavyArmor 
					? 2 
					: 1) 
			: 0;

		public string DamageVsArmor
		{
			get => SelectedWeapon?.DamageVsArmor(ModArmorResilience()).ToString("0.00");
		}

		public string DpsVsArmor
		{
			get => SelectedWeapon?.DpsVsArmor(ModArmorResilience()).ToString("0.00");
		}

		public string MultiplierVsArmor
		{
			get => SelectedWeapon?.Model.GetDamageMultiplier(ModArmorResilience()).ToString("0.00");
		}


		private Task InitializeTask { get; set; }

		private async Task Initialize()
		{
			{
				var wRepo = new WeaponRepository();
				await wRepo.Initialize();
				foreach (var item in wRepo.Weapons)
				{
					var x = new WeaponViewModel(item);
					Weapons.Add(x);
					if (Weapons.Count == 1)
						SelectedWeapon = x;

					// Register eventhandler so when weapons properties are changed.
					// The Calculated properties gets a changed event...
					x.PropertyChanged += (s, e) =>
					{
						if (e.PropertyName == nameof(WeaponViewModel.EnabledModifiers))
						{
							RaiseCalculatedWeaponProptiesChanged();
						}
					};
				}
			}
			{
				var aRepo = new ArmorRepository();
				await aRepo.Initialize();
				foreach (var item in aRepo.Armors)
				{
					ArmorTypes.Add(new ArmorViewModel(item));
					if (ArmorTypes.Count == 1)
						SelectedArmor = ArmorTypes[0];
				}
			}

			{
				var sRepo = new ShieldRepository();
				await sRepo.Initialize();
				foreach (var item in sRepo.Shields)
				{
					ShieldTypes.Add(new ShieldViewModel(item));
					if (ShieldTypes.Count == 1)
						SelectedShield = ShieldTypes[0];
				}
			}

			{
				var hRepo = new ShipHullRepository();
				await hRepo.Initialize();
				foreach (var item in hRepo.Hulls)
				{
					ShipHulls.Add(new ShipHullViewModel(item));
					if (ShipHulls.Count == 1)
						SelectedHull = ShipHulls[0];
				}
			}

			LoadCompleted = true;
		}
	}
}

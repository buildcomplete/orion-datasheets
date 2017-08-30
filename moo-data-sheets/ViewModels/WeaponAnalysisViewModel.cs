using Models;
using Models.Repository;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Uwp;
using LiveCharts.Defaults;

namespace moo_data_sheets.ViewModels
{
	public class WeaponAnalysisViewModel : BindableBase
	{
		public ObservableCollection<WeaponViewModel> Weapons = new ObservableCollection<WeaponViewModel>();

		public ObservableCollection<ArmorViewModel> ArmorTypes = new ObservableCollection<ArmorViewModel>();

		public ObservableCollection<ShieldViewModel> ShieldTypes = new ObservableCollection<ShieldViewModel>();

		public ObservableCollection<ShipHullViewModel> ShipHulls = new ObservableCollection<ShipHullViewModel>();

		//public PlotModel DamagePlot { get; private set; }
		public SeriesCollection SeriesCollection
		{
			get
			{
				double hullStrengthPrevious = -1;
				double hullStrength = SelectedHull != null
					? SelectedHull.Strength * (SelectedShield != null
						? SelectedShield.Multiplier
						: 1)
					: 150;

				double readyT = 0;
				var val = new ChartValues<ObservablePoint>();
				int maxT = 200;
				for (int t = 0; t < maxT; ++t)
				{
					if (t > readyT)
					{
						hullStrength -= SelectedWeapon.DamageVsArmor(SelectedArmor.Rating);
						readyT += SelectedWeapon.Model.Cooldown;
					}
					if (hullStrength < 0)
						hullStrength = 0;

					if (hullStrength != hullStrengthPrevious)
						val.Add(new ObservablePoint(t, hullStrength));
					hullStrengthPrevious = hullStrength;
				}
				val.Add(new ObservablePoint(maxT, hullStrength));


				return new SeriesCollection
				{
					 new StepLineSeries
					 {
						 Values = val,
					 },
					//new ColumnSeries
					//{
					//	Values = new ChartValues<decimal> { 5, 6, 2, 7 }
					//}
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
					|| e.PropertyName == nameof(SelectedHull))
				{
					RaisePropertyChanged(nameof(DamageVsArmor));
					RaisePropertyChanged(nameof(DpsVsArmor));
					RaisePropertyChanged(nameof(SeriesCollection));
				}
			};

			InitializeTask = Initialize();
			InitializeTask.ContinueWith((T) =>
			{
				LoadErrorMessage = T.Exception?.Message;
			});

			//DamagePlot = new PlotModel { Title = "Damage Profile" };
			//DamagePlot.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.05, "cos(x)"));

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

		public string DamageVsArmor { get => SelectedWeapon.DamageVsArmor(SelectedArmor.Rating).ToString("0.00"); }

		public string DpsVsArmor { get => SelectedWeapon.DpsVsArmor(SelectedArmor.Rating).ToString("0.00"); }


		private Task InitializeTask { get; set; }

		private async Task Initialize()
		{
			{
				var wRepo = new WeaponRepository();
				await wRepo.Initialize();
				foreach (var item in wRepo.Weapons)
				{
					Weapons.Add(new WeaponViewModel(item));
					if (Weapons.Count == 1)
						SelectedWeapon = Weapons[0];
				}
			}
			{
				var aRepo = new ArmorRepository();
				await aRepo.Initialize();
				foreach (var item in aRepo.Weapons)
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

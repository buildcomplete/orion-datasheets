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
				if (SelectedHull == null
					|| SelectedShield == null
					|| SelectedArmor == null)
					return null;

				ShipConfiguration shipConfig = new ShipConfiguration(
					SelectedHull.Model,
					SelectedArmor.Model,
					SelectedShield.Model);

				SimulatedCollection s = new SimulatedCollection();
				s.Add(SelectedWeapon.Model);
				s.Add(shipConfig);

				SelectedWeapon.Model.Target = shipConfig;
				
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

		public string DamageVsArmor
		{
			get => SelectedWeapon?.DamageVsArmor(SelectedArmor != null ? SelectedArmor.Resilience : 0).ToString("0.00");
		}

		public string DpsVsArmor
		{
			get => SelectedWeapon?.DpsVsArmor(SelectedArmor!= null ? SelectedArmor.Resilience : 0).ToString("0.00");
		}


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

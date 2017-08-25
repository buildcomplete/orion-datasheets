using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace moo_data_sheets
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public ViewModels.PlanetViewModel PlanetViewModel = new ViewModels.PlanetViewModel(new Models.Planet { Biome = "Terran" });

		public ObservableCollection<ViewModels.PlanetViewModel> Planets = 
			new ObservableCollection<ViewModels.PlanetViewModel>();

		public ViewModels.PlanetViewModel SelectedPlanet
		{
			get => this.GetValue(SelectedPlanetProperty) as ViewModels.PlanetViewModel;
			set => this.SetValue(SelectedPlanetProperty, value as ViewModels.PlanetViewModel);
		}

		public static readonly DependencyProperty SelectedPlanetProperty = DependencyProperty.Register(
				nameof(SelectedPlanet), 
				typeof(ViewModels.PlanetViewModel), 
				typeof(MainPage), 
				new PropertyMetadata(false));

		public MainPage()
		{
			this.InitializeComponent();

			Action<Planet> AddPlanet = (P) =>
				 Planets.Add(new ViewModels.PlanetViewModel(P));

			AddPlanet(new Planet
			{
				Biome = "Arid",
				Stats = new Dictionary<PlanetSize, Stats>
				{
					{ PlanetSize.Huge, new Stats { MaxPop = 8, Food = "555430000" } },
					{ PlanetSize.Large, new Stats { MaxPop = 8, Food = "555430000" } }
				}
			});
			AddPlanet(new Planet { Biome = "Barren" });
			AddPlanet(new Planet { Biome = "Cavernous" });
			AddPlanet(new Planet { Biome = "Desert" });
			AddPlanet(new Planet { Biome = "Gaia" });
			AddPlanet(new Planet { Biome = "Gas_giant" });
			AddPlanet(new Planet { Biome = "Grassland" });
			AddPlanet(new Planet { Biome = "Inferno" });
			AddPlanet(new Planet { Biome = "Ocean" });
			AddPlanet(new Planet { Biome = "Radiated" });
			AddPlanet(new Planet { Biome = "Swamp" });
			AddPlanet(new Planet { Biome = "Terran" });
			AddPlanet(new Planet { Biome = "Toxic" });
			AddPlanet(new Planet { Biome = "Tropical" });
			AddPlanet(new Planet { Biome = "Tundra" });
			AddPlanet(new Planet { Biome = "Volcanic" });

			SelectedPlanet = Planets[0];
		}
	}
}
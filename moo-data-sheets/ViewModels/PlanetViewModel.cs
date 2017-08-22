using Prism.Mvvm;
using System;
using System.Linq;

namespace moo_data_sheets.ViewModels
{
	public class PlanetViewModel : BindableBase
	{
		static Random r = new Random();
		public PlanetViewModel(Models.Planet planet)
		{
			int[] values = new int[5].Select(X => r.Next(5)).OrderByDescending(X => X).ToArray();

			PlanetFood = string.Join("/", values);
			Planet = planet;
		}

		public override string ToString()
		{
			return $"PlanetVM - Planet.Biome {Planet?.Biome}";
		}

		public string PlanetFood { get; set; }

		private Models.Planet _planet;
		public Models.Planet Planet
		{
			get => _planet;
			set => SetProperty(ref _planet, value);
		}

		public string PlanetBiome { get => _planet?.Biome; }

		public string ImageFile
		{
			get => _planet == null
				? null
				: $"/Assets/planets/planet_{_planet.Biome.ToLower()}.png";
		}
	}
}

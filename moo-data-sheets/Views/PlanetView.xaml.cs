using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace moo_data_sheets
{
	public sealed partial class PlanetView : UserControl
	{

		public ViewModels.PlanetViewModel ViewModel
		{
			get => this.GetValue(ViewModelProperty) as ViewModels.PlanetViewModel;
			set => this.SetValue(ViewModelProperty, value as ViewModels.PlanetViewModel);
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
				nameof(PlanetView),
				typeof(ViewModels.PlanetViewModel),
				typeof(ViewModels.PlanetViewModel),
				new PropertyMetadata(false));


		public PlanetView()
		{
			this.InitializeComponent();
		}
	}
}
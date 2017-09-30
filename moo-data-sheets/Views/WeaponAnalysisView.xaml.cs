using moo_data_sheets.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace moo_data_sheets.Views
{
	public sealed partial class WeaponAnalysisView : UserControl
	{
		public WeaponAnalysisViewModel ViewModel = new WeaponAnalysisViewModel();
		
		public WeaponAnalysisView()
		{
			this.InitializeComponent();
			_plot.SeriesColors = new LiveCharts.Uwp.ColorsCollection();
			_plot.SeriesColors.Add(Colors.Red);
			_plot.SeriesColors.Add(Colors.Blue);
		}

	}
}

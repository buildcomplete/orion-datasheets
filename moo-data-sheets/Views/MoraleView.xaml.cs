using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
	public sealed partial class MoraleView : UserControl
	{
		public MoraleView()
		{
			this.InitializeComponent();

			MoraleGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Header
			MoraleGrid.RowDefinitions.Add(new RowDefinition()); // Header

			// Create columns
			List<int> morales = new List<int>();
			for (int i = 100; i > 40; i -= 5)
				morales.Add(i);

			foreach (var item in morales)
				MoraleGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Header

			// Create Rows
			List<int> population = new List<int>();
			for (int i = 1; i <= 20; i++)
				population.Add(i);

			foreach (var item in population)
				MoraleGrid.RowDefinitions.Add(new RowDefinition()); // Header



			for (int i = 1; i < MoraleGrid.ColumnDefinitions.Count; i++)
			{
				var border = new Border { Background = new SolidColorBrush(Colors.Transparent) };
				MoraleGrid.Children.Add(border);

				Grid.SetColumn(border, i);
				Grid.SetRow(border, 1);
				Grid.SetRowSpan(border, MoraleGrid.RowDefinitions.Count - 1);
				border.PointerEntered += (o, e) =>
					border.Background = new SolidColorBrush(Colors.Tomato);

				border.PointerExited += (o, e) =>
					border.Background = new SolidColorBrush(Colors.Transparent);
			}

			for (int i = 1; i < MoraleGrid.RowDefinitions.Count; i++)
			{
				var border = new Border { Background = new SolidColorBrush(Colors.Transparent) };
				MoraleGrid.Children.Add(border);

				Grid.SetColumn(border, 1);
				Grid.SetRow(border, i);
				Grid.SetColumnSpan(border, MoraleGrid.ColumnDefinitions.Count - 1);
				border.PointerEntered += (o, e) =>
					border.Background = new SolidColorBrush(Colors.Tomato);

				border.PointerExited += (o, e) =>
					border.Background = new SolidColorBrush(Colors.Transparent);
			}


			// Function for creating a textblock with center alignment
			// I wonder if this could be made in xaml
			Func<string, TextBlock> CreateTextBlockCenteredText = (text) =>
				new TextBlock
				{
					Text = text,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center
				};

			// Fill rows header with populations
			for (int i = 0; i < population.Count; ++i)
			{
				var item = CreateTextBlockCenteredText(population[i].ToString());
				item.FontWeight = Windows.UI.Text.FontWeights.Bold;

				Grid.SetRow(item, i + 1);
				MoraleGrid.Children.Add(item);
			}

			// Fill column header with morale
			for (int i = 0; i < morales.Count; ++i)
			{
				var item = CreateTextBlockCenteredText(morales[i].ToString() + "%");
				item.FontWeight = Windows.UI.Text.FontWeights.Bold;

				Grid.SetColumn(item, i + 1);
				MoraleGrid.Children.Add(item);
			}

			// Function for creating the color I like for the morale table as function
			// of the strike size.
			Func<int, Border> getStrikeSizeBorder = (strike) =>
			{
				Dictionary<int, Brush> colors = new Dictionary<int, Brush>()
				{
					{0, new SolidColorBrush(Windows.UI.Color.FromArgb(155, 183, 255, 205)) },
					{1, new SolidColorBrush(Windows.UI.Color.FromArgb(155, 252, 232, 178)) },
					{2, new SolidColorBrush(Windows.UI.Color.FromArgb(155, 244, 199, 195)) },
					{3, new SolidColorBrush(Windows.UI.Color.FromArgb(155, 255, 153, 0)) }
				};

				return new Border
				{
					Background = colors.ContainsKey(strike)
						? colors[strike]
						: new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 255))
				};
			};

			// Fill the table with the actual values
			// The trick is the casting to int the floors the value.
			for (int i = 0; i < morales.Count; ++i)
			{
				for (int j = 0; j < population.Count; ++j)
				{
					int strikeSize = (100 - morales[i]) * population[j] / 100;

					var item = getStrikeSizeBorder(strikeSize);
					item.Child = CreateTextBlockCenteredText(strikeSize.ToString()); ;

					Grid.SetRow(item, j + 1);
					Grid.SetColumn(item, i + 1);
					//MoraleGrid.Children.Add(item);
				}
			}
		}
	}
}

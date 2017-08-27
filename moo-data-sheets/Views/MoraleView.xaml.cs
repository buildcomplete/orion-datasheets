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
			Action<int, Border, TextBlock> applyStrikeStyle = (strike, border, textBlock) =>
			{
				Dictionary<int, Brush> bgColors = new Dictionary<int, Brush>()
				{
					{0, new SolidColorBrush(Color.FromArgb(155, 183, 255, 205)) },
					{1, new SolidColorBrush(Color.FromArgb(155, 252, 232, 178)) },
					{2, new SolidColorBrush(Color.FromArgb(155, 244, 199, 195)) },
					{3, new SolidColorBrush(Color.FromArgb(155, 255, 153, 0)) }
				};

				border.Background = bgColors.ContainsKey(strike)
						? bgColors[strike]
						: new SolidColorBrush(Color.FromArgb(255, 255, 0, 255));
				textBlock.Foreground = strike < 4
					? new SolidColorBrush(Colors.Black)
					: new SolidColorBrush(Colors.Yellow);


				border.BorderBrush = new SolidColorBrush(Colors.Black);

			};

			List<Border> activeElements = new List<Border>();
			// Fill the table with the actual values
			// The trick is the casting to int the floors the value.
			for (int i = 0; i < morales.Count; ++i)
			{
				for (int j = 0; j < population.Count; ++j)
				{
					int strikeSize = ((100 - morales[i]) * population[j] / 100);

					var item = new Border();
					activeElements.Add(item);

					var textItem = CreateTextBlockCenteredText(strikeSize.ToString());
					item.Child = textItem;
					Grid.SetRow(item, j + 1);
					Grid.SetColumn(item, i + 1);
					MoraleGrid.Children.Add(item);

					applyStrikeStyle(strikeSize, item, textItem);

				}
			}

			Stack<Border> ModifiedBorders = new Stack<Border>();
			Action<Border> AddHighLight = (I) =>
			{
				var leftOfI = activeElements
					.Where(X =>
						Grid.GetColumn(X) < Grid.GetColumn(I)
						&& Grid.GetRow(X) == Grid.GetRow(I))
					.ToArray();

				var aboveI = activeElements
					.Where(X =>
						Grid.GetColumn(X) == Grid.GetColumn(I)
						&& Grid.GetRow(X) < Grid.GetRow(I))
					.ToArray();

				var w = 0.5;
				var neutralBorderThickness = new Thickness();
				var aboveBorderThickness = new Thickness(w, 0, w, 0);
				var leftBorderThickness = new Thickness(0, w, 0, w);
				var actualFocusBorderThickness = new Thickness(0, 0, w, w);

				I.PointerEntered += (o, e) =>
				{
					// Restore state on previously modified borders
					while (ModifiedBorders.Count != 0)
						ModifiedBorders.Pop().BorderThickness = neutralBorderThickness;

					I.BorderThickness = actualFocusBorderThickness;
					ModifiedBorders.Push(I);

					foreach (var item in aboveI)
					{
						item.BorderThickness = aboveBorderThickness;
						ModifiedBorders.Push(item);
					}

					foreach (var item in leftOfI)
					{
						item.BorderThickness = leftBorderThickness;
						ModifiedBorders.Push(item);
					}
				};
			};

			foreach (var item in activeElements)
			{
				AddHighLight(item);
			}
		}
	}
}

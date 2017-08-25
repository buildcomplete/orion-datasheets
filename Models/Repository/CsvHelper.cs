using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Models.Repository
{
	internal static class CsvHelper
	{
		public static async Task<string[]> GetLines(string uri)
		{
			var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

			string text = await FileIO.ReadTextAsync(file);
			return text.Split(new char[] { '\n' });
		}
	}
}

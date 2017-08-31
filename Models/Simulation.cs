using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class SimulatedCollection
	{
		private List<SimulatedObject> objects = new List<SimulatedObject>();
		public void Add(SimulatedObject X) { objects.Add(X); }

		public void Tick(double dt)
		{
			objects.ForEach(X => X.Tick(dt));
		}
	}
}

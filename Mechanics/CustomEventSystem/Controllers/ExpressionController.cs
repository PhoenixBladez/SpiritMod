using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.EventSystem.Controllers
{
	public class ExpressionController : EventController
	{
		private Action _method;

		public ExpressionController(float start, float length, Action method) : base(start, length)
		{
			_method = method;
		}

		public override void Update(float time)
		{
			_method?.Invoke();
		}
	}
}

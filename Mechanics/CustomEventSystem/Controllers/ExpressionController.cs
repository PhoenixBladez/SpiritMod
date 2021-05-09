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
		private int _frames;

		public ExpressionController(float start, Action method) : base(start)
		{
			_method = method;
			_frames = 1;
			AmICompleted = () => _frames <= 0;
		}

		public ExpressionController(float start, int frameCount, Action method) : base(start)
		{
			_method = method;
			_frames = frameCount;
			AmICompleted = () => _frames <= 0;
		}

		public override void Update(float time)
		{
			_method?.Invoke();
			_frames--;
		}
	}
}

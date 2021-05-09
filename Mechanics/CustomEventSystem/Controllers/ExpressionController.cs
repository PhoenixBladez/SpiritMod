using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.EventSystem.Controllers
{
	public class ExpressionController : EventController
	{
		private Action<int> _method;
		private int _frames;
		private int _currentFrame;

		public ExpressionController(float start, Action<int> method) : base(start)
		{
			_method = method;
			_frames = 1;
			AmICompleted = () => _currentFrame >= _frames;
		}

		public ExpressionController(float start, int frameCount, Action<int> method) : base(start)
		{
			_method = method;
			_frames = frameCount;
			AmICompleted = () => _currentFrame >= _frames;
		}

		public override void Update(float time)
		{
			_method?.Invoke(_currentFrame++);
			_frames--;
		}
	}
}

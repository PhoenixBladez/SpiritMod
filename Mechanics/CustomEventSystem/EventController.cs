using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.EventSystem
{
	public abstract class EventController
	{
		public float StartTime { get; set; }
		public float Length { get; set; }
		public Func<bool> CheckIfCompleted { get; set; }

		protected float _progress;

		public EventController(float start, float length)
		{
			StartTime = start;
			Length = length;
			CheckIfCompleted = () => _progress >= 1f;
		}

		public bool DoUpdate(float time)
		{
			_progress = (time - StartTime) / Length;

			Update(time);

			return CheckIfCompleted();
		}

		public virtual void Reset()
		{
			_progress = 0f;
		}

		public virtual void Activate() { }
		public virtual void Deactivate() { }
		public virtual void Update(float time) { }
	}
}

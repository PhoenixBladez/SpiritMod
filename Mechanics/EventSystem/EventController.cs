using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Priority_Queue;

namespace SpiritMod.Mechanics.EventSystem
{
	public abstract class EventController : FastPriorityQueueNode
	{
		public float StartTime { get; set; }
		/// <summary>
		/// A function used to determine whether the controller is completed or not.
		/// </summary>
		public Func<bool> AmICompleted { get; set; }

		public EventController(float start)
		{
			StartTime = start;
			AmICompleted = () => false;
		}

		public bool DoUpdate(float time)
		{
			Update(time);

			return AmICompleted();
		}

		public virtual void Reset() { }
		public virtual void Activate() { }
		public virtual void Deactivate() { }
		public virtual void Update(float time) { }
	}
}

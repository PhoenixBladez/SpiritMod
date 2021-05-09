using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Priority_Queue;

namespace SpiritMod.Mechanics.EventSystem
{
	public abstract class Event
	{
		private FastPriorityQueue<EventController> _controllerQueue;
		protected List<EventController> _currentlyPlaying;
		protected float _currentTime;
		private bool _playedLast;

		public Event()
		{
			_controllerQueue = new FastPriorityQueue<EventController>(256);
			_currentlyPlaying = new List<EventController>();
		}

		protected void AddToQueue(EventController controller)
		{
			_controllerQueue.Enqueue(controller, controller.StartTime);
		}

		public virtual void Play() { }

		/// <returns>true if the cutscene is finished.</returns>
		public virtual bool Update(float deltaTime)
		{
			_currentTime += deltaTime;

			// if there's no controllers playing, and there's no controller upcoming, we're done!
			if (_playedLast && _currentlyPlaying.Count == 0) return true;

			// update currently playing controllers
			for (int i = 0; i < _currentlyPlaying.Count; i++)
			{
				if (_currentlyPlaying[i].DoUpdate(_currentTime))
				{
					_currentlyPlaying[i].Deactivate();
					_currentlyPlaying[i].Reset();
					_currentlyPlaying.RemoveAt(i--);
				}
			}

			// update which controllers should now play
			if (_controllerQueue.Count > 0)
			{
				while (!_playedLast && _currentTime >= _controllerQueue.First.StartTime)
				{
					var controller = _controllerQueue.Dequeue();
					_currentlyPlaying.Add(controller);
					controller.Activate();

					if (_controllerQueue.Count == 0)
					{
						_playedLast = true;
						break;
					}
				}
			}

			return false;
		}
	}
}

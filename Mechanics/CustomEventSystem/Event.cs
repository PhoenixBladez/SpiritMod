using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.EventSystem
{
	public abstract class Event
	{
		protected LinkedList<EventController> _controllers;
		protected LinkedListNode<EventController> _currentController;
		protected List<EventController> _currentlyPlaying;
		protected float _currentTime;
		private bool _playedLast;

		public Event()
		{
			_controllers = new LinkedList<EventController>();
			_currentlyPlaying = new List<EventController>();
		}

		public virtual void Play()
		{
			_currentController = _controllers.First;
		}

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
			while (!_playedLast && _currentTime >= _currentController.Value.StartTime)
			{
				_currentlyPlaying.Add(_currentController.Value);
				_currentController.Value.Activate();

				if (_currentController.Next == null)
				{
					_playedLast = true;
					break;
				}
				_currentController = _currentController.Next;
			}

			return false;
		}
	}
}

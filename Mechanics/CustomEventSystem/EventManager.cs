using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Mechanics.EventSystem
{
	public static class EventManager
	{
		private static List<Event> _activeEvents;

		public static void Load()
		{
			_activeEvents = new List<Event>();
			On.Terraria.Main.DoUpdate += Main_DoUpdate;
		}

		public static void Unload()
		{
			_activeEvents = null;
			On.Terraria.Main.DoUpdate -= Main_DoUpdate;
		}

		private static void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Terraria.Main self, GameTime gameTime)
		{
			orig(self, gameTime);

			// update cutscene
			Update(gameTime);
		}

		public static bool IsPlaying<T>()
		{
			foreach (Event e in _activeEvents)
			{
				if (e is T) return true;
			}
			return false;
		}

		public static void Update(GameTime gameTime)
		{
			if (Main.gameMenu)
			{
				_activeEvents.Clear();
				return;
			}

			for (int i = 0; i < _activeEvents.Count; i++)
			{
				if (_activeEvents[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds))
				{
					_activeEvents.RemoveAt(i--);
				}
			}
		}

		public static void PlayEvent(Event scene)
		{
			scene.Play();

			_activeEvents.Add(scene);
		}
	}
}

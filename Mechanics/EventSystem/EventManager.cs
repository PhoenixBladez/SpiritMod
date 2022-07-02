﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Mechanics.EventSystem
{
	public static class EventManager
	{
		private static List<Event> _activeEvents;

		static EventManager()
		{
			_activeEvents = new List<Event>();
		}

		public static void Load()
		{
			_activeEvents = new List<Event>();
			On.Terraria.Main.DoUpdate += Main_DoUpdate;
			On.Terraria.Graphics.Effects.OverlayManager.Draw += OverlayManager_Draw; 
		}

		public static void Unload()
		{
			On.Terraria.Main.DoUpdate -= Main_DoUpdate;
    		On.Terraria.Graphics.Effects.OverlayManager.Draw -= OverlayManager_Draw;
    		_activeEvents = null;
		}

		private static void OverlayManager_Draw(On.Terraria.Graphics.Effects.OverlayManager.orig_Draw orig, Terraria.Graphics.Effects.OverlayManager self, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Terraria.Graphics.Effects.RenderLayers layer, bool beginSpriteBatch)
		{
			orig(self, spriteBatch, layer, beginSpriteBatch);

			if (_activeEvents != null)
			{
				foreach (Event e in _activeEvents)
				{
					e.DrawAtLayer(spriteBatch, layer, beginSpriteBatch);
				}
			}
		}

		private static void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
		{
			orig(self, ref gameTime);

			if (_activeEvents != null)
			{
				// update cutscene
				Update(gameTime);
			}
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
					_activeEvents[i].Deactivate();
					_activeEvents.RemoveAt(i--);
				}
			}
		}

		public static void PlayEvent(Event scene)
		{
			scene.Activate();

			_activeEvents.Add(scene);
		}
	}
}

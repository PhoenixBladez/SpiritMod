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
		private static List<Event> _activeCutscenes;

		public static void Load()
		{
			_activeCutscenes = new List<Event>();
			On.Terraria.Main.DoUpdate += Main_DoUpdate;
		}

		public static void Unload()
		{
			_activeCutscenes = null;
			On.Terraria.Main.DoUpdate -= Main_DoUpdate;
		}

		private static void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Terraria.Main self, GameTime gameTime)
		{
			orig(self, gameTime);

			// update cutscene
			Update(gameTime);
		}

		public static void Update(GameTime gameTime)
		{
			if (Main.gameMenu)
			{
				_activeCutscenes.Clear();
				return;
			}

			for (int i = 0; i < _activeCutscenes.Count; i++)
			{
				if (_activeCutscenes[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds))
				{
					_activeCutscenes.RemoveAt(i--);
				}
			}
		}

		public static void PlayEvent(Event scene)
		{
			scene.Play();

			_activeCutscenes.Add(scene);
		}
	}
}

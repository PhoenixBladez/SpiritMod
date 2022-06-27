using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using System.Linq;

namespace SpiritMod.Mechanics.PortraitSystem
{
	public class PortraitManager
	{
		private static readonly Dictionary<int, BasePortrait> portraits = new Dictionary<int, BasePortrait>();
		private static readonly List<ModCallPortrait> callPortraits = new List<ModCallPortrait>();

		public static void Load() //Load all portraits and add them to the dict
		{
			var types = typeof(BasePortrait).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (type.IsSubclassOf(typeof(BasePortrait)) && !type.IsAbstract)
				{
					var p = Activator.CreateInstance(type, true) as BasePortrait;
					portraits.Add(p.ID, p);
				}
			}
		}

		public static void Unload()
		{
			portraits.Clear();
			callPortraits.Clear();
		}

		public static BasePortrait GetPortrait(int ID) => portraits[ID];
		public static bool HasPortrait(int ID) => portraits.ContainsKey(ID);

		public static ModCallPortrait GetCallPortrait(int ID) => callPortraits.First(x => x.ID == ID);
		public static bool HasCallPortrait(int ID) => callPortraits.Any(x => x.ID == ID);

		internal static void ModCallAddPortrait(object[] args)
		{
			//args[0] should be "Portrait"
			//args[1] should be the mod it's coming from
			if (!(args[1] is Mod))
			{
				var stack = new System.Diagnostics.StackTrace(true);
				SpiritMod.Instance.Logger.Error("Call Error: Mod.Call Portrait does not contain valid Mod value:\n" + stack.ToString());
				return;
			}
			var callerMod = args[1] as Mod;

			//args[2] should the portrait NPC's internal name
			string name = args[2].ToString();
			int id = callerMod.Find<ModNPC>(name).Type;

			//args[3] should be the portrait's texture
			if (!(args[3] is Texture2D))
			{
				var stack = new System.Diagnostics.StackTrace(true);
				SpiritMod.Instance.Logger.Error("Call Error: Mod.Call Portrait does not contain valid Texture value:\n" + stack.ToString());
				return;
			}
			var texture = args[3] as Texture2D;

			Func<string, NPC, Rectangle> getFrame = delegate { return new Rectangle(0, 0, 108, 108); }; //Default to a basic, single frame portrait
			if (args.Length > 4) //Optional argument
			{
				//args[4] should be the portrait's Get Frame alias
				if (args[4] != null && !(args[4] is Func<string, NPC, Rectangle>))
				{
					var stack = new System.Diagnostics.StackTrace(true);
					SpiritMod.Instance.Logger.Error("Call Error: Mod.Call Portrait does not contain valid GetFrame delegate:\n" + stack.ToString());
					return;
				}
				if (args[4] != null) //Allow end user to ignore this argument
					getFrame = args[4] as Func<string, NPC, Rectangle>;
			}

			var size = new Point(108, 108);
			if (args.Length > 5) //Optional argument
			{
				//args[5] should be the portrait's BaseSize
				if (args[5] != null && !(args[5] is Point))
				{
					var stack = new System.Diagnostics.StackTrace(true);
					SpiritMod.Instance.Logger.Error("Call Error: Mod.Call Portrait does not contain valid Point value:\n" + stack.ToString());
					return;
				}
				if (args[5] != null) //Allow end user to ignore this argument
					size = (Point)args[5];
			}

			var portrait = new ModCallPortrait(id, texture, getFrame, size);
			callPortraits.Add(portrait);

			// args[0] should be "Portrait" (string)
			// args[1] should be the mod it's coming from
			// args[2] should the portrait NPC's internal name
			// args[3] should be the portrait's texture
			// args[4] should be the portrait's Get Frame hook, if any (defaults to a single frame portrait)
			// args[5] should be the portrait's BaseSize, if any defaults to (108, 108)

			// So calling this would be
			// spiritMod.Call("Portrait", myMod, "MyTownNPC", ModContent.Request<Texture2D>("SomeTexture")); for a default, single frame portrait.
			//
			// Or, 
			//
			// spiritMod.Call("Portrait", myMod, "MyTownNPC", ModContent.Request<Texture2D>("SomeTexture"),
			//		(string text, NPC talkNPC) =>
			//		{
			//			if (text.ToUpper() == "USE THAT ONE COOL FRAME PLEASE")
			//				return new Rectangle(110, 0, 108, 108);
			//			return new Rectangle(0, 0, 108, 108);
			//		}); //For a portrait with two frames. Etc.
		}
	}
}

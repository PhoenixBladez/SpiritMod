using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace SpiritMod.Tide
{
	public class TideWorld : ModWorld
	{
		public static int TidePoints = 0;
		public static int TidePoints2;
		public static bool TheTide;
		public static bool InBeach;

		public override void Initialize()
		{
			InBeach = false;
			TheTide = false;
			TidePoints2 = 0;
		}

		public override void PostUpdate()
		{
			if (TidePoints2 >= 80 || TidePoints >= 80)
			{
				Main.NewText("The Tide has waned!", 85, 172, 247);
				TidePoints2 = 0;
				TidePoints = 0;
				TheTide = false;
			}
		}

	}
}

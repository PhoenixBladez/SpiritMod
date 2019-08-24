using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Tide
{
	public class TidePlayer : ModPlayer
	{
		public override void PostUpdate()
		{

			if (!TideWorld.TheTide)
			{
				TideWorld.TidePoints2 = 0;
			}
			if (TideWorld.TheTide)
			{
				if (player.ZoneBeach)
				{
					TideWorld.InBeach = true;
				}
				else
				{
					TideWorld.InBeach = false;
				}
				//const int spawnOffsetX = 600;
				//const int spawnOffsetY = 600;
				//if (Main.rand.Next(1200) == 1 && !NPC.downedMechBossAny)
				//NPC.NewNPC((int)player.Center.X + spawnOffsetX, (int)player.Center.Y, mod.NPCType("Clamper"));
				//if (Main.rand.Next(1200) == 1 && !NPC.downedMechBossAny)
				//NPC.NewNPC((int)player.Center.X - spawnOffsetX, (int)player.Center.Y, mod.NPCType("Clamper"));
				//Stuff for manually spawning if spawning is broken
			}
			TideWorld.TidePoints = TideWorld.TidePoints2;
			if (TideWorld.TidePoints2 >= 100)
			{
				Main.NewText("The Tide has waned!", 39, 86, 134);
				TideWorld.TidePoints2 = 0;
				TideWorld.TheTide = false;
			}
		}
	}
}

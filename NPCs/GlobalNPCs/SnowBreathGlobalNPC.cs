using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.GlobalNPCs
{
	public class SnowBreathGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		int timeAlive;

		public override void PostAI(NPC npc)
		{
			Player closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

			if (closest.ZoneSnow)
			{
				if (npc.townNPC && Main.rand.NextBool(27))
				{
					Vector2 spawnPos = new Vector2(npc.position.X + 8 * npc.direction, npc.Center.Y - 13f);
					int d = Dust.NewDust(spawnPos, npc.width, 10, ModContent.DustType<FrostBreath>(), 1.5f * npc.direction, 0f, 100, default, Main.rand.NextFloat(.7f, 1.7f));
					Main.dust[d].velocity.Y *= 0f;
				}
			}
		}
	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Cystal
{
	public class Cystal_GlobalNPC : GlobalNPC
	{
		public override void AI(NPC npc)
		{
			Player player = Main.player[npc.target];
			for (int i = 0; i < 200; i++)
			{
				NPC npc2 = Main.npc[i];
				if ((double)Vector2.Distance(npc.Center, npc2.Center) < (double)1200f && npc.type == mod.NPCType("Cystal") && npc2.life < npc2.lifeMax - 10 && !npc2.boss && npc2.active && !npc2.friendly && npc2.type != mod.NPCType("Cystal") && npc2.type != mod.NPCType("Cystal_Shield"))
				{
					npc.ai[1] = npc2.whoAmI;
				}
			}
		}
	}
}
using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.NPCs
{
	public class BunnyP : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BunnyP");
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 0;
			npc.alpha = 255;
			npc.value = 0f;
			npc.knockBackResist = 0f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon ? 7f : 0f;
		}

		public override void AI()
		{
			int bunny = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 46, 0, 2, 1, 0, npc.whoAmI, npc.target);
			NPC newProj2 = Main.npc[bunny];
			newProj2.friendly = false;
			npc.life = 0;
		}

	}
}

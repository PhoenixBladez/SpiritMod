using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	public class MoonGazer : ModNPC
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Blood Gazer");
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.aiStyle = -1;
			npc.height = 68;
			npc.damage = 50;
			npc.defense = 13;
			npc.lifeMax = 1500;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			Main.npcFrameCount[npc.type] = 7;
			npc.npcSlots = 5;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.boss = true;
		}

		public override void NPCLoot()
		{
			npc.DropItem(mod.ItemType("Veinstone"), Main.rand.Next(10, 26));
		}

		public override void AI()
		{
			timer++;
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (timer == 50)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				npc.velocity.Y = direction.Y * 10f;
				npc.velocity.X = direction.X * 10f;
				timer = 0;
			}
			if (timer == 40)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				npc.velocity.Y = direction.Y * 1f;
				npc.velocity.X = direction.X * 1f;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}

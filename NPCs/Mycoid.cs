using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Mycoid : ModNPC
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycoid");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 46;
			npc.damage = 16;
			npc.defense = 8;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 6760f;
			npc.knockBackResist = 0.54f;
			npc.aiStyle = 3;
			aiType = NPCID.LacBeetle;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				return spawnInfo.player.GetModPlayer<MyPlayer>(mod).ZoneReach && !Main.dayTime && NPC.downedBoss2 ? 0.08f : 0f;
			}
			return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.08f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		private int Counter;
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.145f, 0.288f, 0.043f);

			timer++;
			if (timer >= 200)
			{
				Counter++;
				if (Counter > 75)
				{
					npc.TargetClosest();
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					float ai = Main.rand.Next(100);
					direction.Normalize();
					int MechBat = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -6, mod.ProjectileType("MycoidSpore"), 10, 0);
					Counter = 0;
				}

				if (Main.rand.Next(2) == 0)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 44);
					Main.dust[dust].scale = .2f;
				}

			}

			if (timer >= 400)
			{
				timer = 0;
			}
			npc.spriteDirection = npc.direction;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
			}
		}
	}
}

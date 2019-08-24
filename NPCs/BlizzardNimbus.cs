using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class BlizzardNimbus : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Nimbus");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.damage = 48;
			npc.width = 40; //324
			npc.height = 54; //216
			npc.defense = 18;
			npc.lifeMax = 220;
			npc.knockBackResist = 0.3f;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 2, 0, 0);
			npc.HitSound = SoundID.NPCHit30;
			npc.DeathSound = SoundID.NPCDeath49;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			float num1164 = 4f;
			float num1165 = 0.75f;
			Vector2 vector133 = new Vector2(npc.Center.X, npc.Center.Y);
			float num1166 = Main.player[npc.target].Center.X - vector133.X;
			float num1167 = Main.player[npc.target].Center.Y - vector133.Y - 200f;
			float num1168 = (float)Math.Sqrt((double)(num1166 * num1166 + num1167 * num1167));
			if (num1168 < 20f)
			{
				num1166 = npc.velocity.X;
				num1167 = npc.velocity.Y;
			}
			else
			{
				num1168 = num1164 / num1168;
				num1166 *= num1168;
				num1167 *= num1168;
			}
			if (npc.velocity.X < num1166)
			{
				npc.velocity.X = npc.velocity.X + num1165;
				if (npc.velocity.X < 0f && num1166 > 0f)
				{
					npc.velocity.X = npc.velocity.X + num1165 * 2f;
				}
			}
			else if (npc.velocity.X > num1166)
			{
				npc.velocity.X = npc.velocity.X - num1165;
				if (npc.velocity.X > 0f && num1166 < 0f)
				{
					npc.velocity.X = npc.velocity.X - num1165 * 2f;
				}
			}
			if (npc.velocity.Y < num1167)
			{
				npc.velocity.Y = npc.velocity.Y + num1165;
				if (npc.velocity.Y < 0f && num1167 > 0f)
				{
					npc.velocity.Y = npc.velocity.Y + num1165 * 2f;
				}
			}
			else if (npc.velocity.Y > num1167)
			{
				npc.velocity.Y = npc.velocity.Y - num1165;
				if (npc.velocity.Y > 0f && num1167 < 0f)
				{
					npc.velocity.Y = npc.velocity.Y - num1165 * 2f;
				}
			}
			if (npc.position.X + (float)npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != 1)
			{
				npc.ai[0] += 4f;
				if (npc.ai[0] > 32f)
				{
					npc.ai[0] = 0f;
					int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
					int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
					int num184 = 26;
					if (Main.expertMode)
					{
						num184 = 14;
					}
					Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 5f, ProjectileID.FrostShard, num184, 0f, Main.myPlayer, 0f, 0f);
					return;
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 14, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrigidWind"));
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.sky && Main.hardMode ? 0.16f : 0f;
		}
	}
}
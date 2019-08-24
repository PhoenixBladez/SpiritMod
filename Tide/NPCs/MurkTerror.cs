using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class MurkTerror : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murky Terror");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{

			npc.damage = 34;
			npc.width = 66; //324
			npc.height = 54; //216
			npc.defense = 15;
			npc.lifeMax = 160;
			npc.knockBackResist = 0.45f;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath5;
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
				if (npc.ai[0] > 256f)
				{
					npc.ai[0] = 0f;
					int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
					int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
					int num184 = 15;
					if (Main.expertMode)
					{
						num184 = 20;
					}
					Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 5f, mod.ProjectileType("WitherBolt"), num184, 0f, Main.myPlayer, 0f, 0f);
					return;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void NPCLoot()
		{
			{
				if (Main.rand.Next(2) == 0 && !NPC.downedMechBossAny)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PearlFragment"), 1);
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DreadWater"), 1);
				}
			}
			if (Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlackTide"), 1);
			}

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murkhead"), 1f);
				if (TideWorld.TheTide)
				{
					TideWorld.TidePoints2 += 1;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach)
				return 2f;
			else if (TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBossAny)
				return 1f;
			return 0;
		}
	}
}
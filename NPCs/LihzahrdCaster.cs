using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class LihzahrdCaster : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Priest");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 60;

			npc.lifeMax = 320;
			npc.defense = 16;
			npc.damage = 42;

			npc.HitSound = SoundID.NPCHit26;
			npc.DeathSound = SoundID.NPCDeath2;

			npc.value = 900f;
			npc.knockBackResist = 0.35f;

			npc.netAlways = true;
			npc.chaseable = true;
			npc.lavaImmune = true;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SunShard"));

			if (Main.rand.Next(Main.expertMode ? 13 : 20) < 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LihzahrdCasterStaff"));
			}
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.velocity.X = npc.velocity.X * 0.93f;
			if (npc.velocity.X > -0.1F && npc.velocity.X < 0.1F)
				npc.velocity.X = 0;
			if (npc.ai[0] == 0)
				npc.ai[0] = 500f;

			if (npc.ai[2] != 0 && npc.ai[3] != 0)
			{
				// Teleport effects: away.
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 244, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
				npc.position.X = (npc.ai[2] * 16 - (npc.width / 2) + 8);
				npc.position.Y = npc.ai[3] * 16f - npc.height;
				npc.velocity.X = 0.0f;
				npc.velocity.Y = 0.0f;
				npc.ai[2] = 0.0f;
				npc.ai[3] = 0.0f;
				// Teleport effects: arrived.
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 244, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
			}

			++npc.ai[0];

			if (npc.ai[0] == 100 || npc.ai[0] == 200 || npc.ai[0] == 300)
			{
				npc.ai[1] = 30f;
				npc.netUpdate = true;
			}

			bool teleport = false;
			// Teleport
			if (npc.ai[0] >= 500 && Main.netMode != 1)
			{
				teleport = true;
			}

			if (teleport)
				Teleport();

			if (npc.ai[1] > 0)
			{
				--npc.ai[1];
				if (npc.ai[1] == 15)
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
					if (Main.netMode != 1)
					{
						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						direction.X *= 14f;
						direction.Y *= 14f;

						int amountOfProjectiles = Main.rand.Next(1, 2);
						for (int i = 0; i < amountOfProjectiles; ++i)
						{
							float A = (float)Main.rand.Next(-50, 50) * 0.02f;
							float B = (float)Main.rand.Next(-50, 50) * 0.02f;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("SunBlast"), npc.damage / 3 * 2, 1, Main.myPlayer, 0, 0);
						}
					}
				}
			}

			if (Main.rand.Next(3) == 0)
				return false;
			Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 244, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
			dust.noGravity = true;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

			return false;
		}

		public void Teleport()
		{
			npc.ai[0] = 1f;
			int num1 = (int)Main.player[npc.target].position.X / 16;
			int num2 = (int)Main.player[npc.target].position.Y / 16;
			int num3 = (int)npc.position.X / 16;
			int num4 = (int)npc.position.Y / 16;
			int num5 = 20;
			int num6 = 0;
			bool flag1 = false;
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0)
			{
				num6 = 100;
				flag1 = true;
			}
			while (!flag1 && num6 < 100)
			{
				++num6;
				int index1 = Main.rand.Next(num1 - num5, num1 + num5);
				for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
				{
					if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive())
					{
						bool flag2 = true;
						if (Main.tile[index1, index2 - 1].lava())
							flag2 = false;
						if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
						{
							npc.ai[1] = 20f;
							npc.ai[2] = (float)index1;
							npc.ai[3] = (float)index2;
							flag1 = true;
							break;
						}
					}
				}
			}
			npc.netUpdate = true;
		}

		public override void FindFrame(int frameHeight)
		{
			int currShootFrame = (int)npc.ai[1];
			if (currShootFrame >= 25)
				npc.frame.Y = frameHeight;
			else if (currShootFrame >= 20)
				npc.frame.Y = frameHeight * 2;
			else if (currShootFrame >= 15)
				npc.frame.Y = frameHeight * 3;
			else if (currShootFrame >= 10)
				npc.frame.Y = frameHeight * 2;
			else if (currShootFrame >= 5)
				npc.frame.Y = frameHeight;
			else
				npc.frame.Y = 0;

			npc.spriteDirection = npc.direction;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 10; i++)
				;
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 244, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !NPC.downedPlantBoss)
			{
				return 0f;
			}
			return SpawnCondition.JungleTemple.Chance * 0.19457f;
		}
	}
}
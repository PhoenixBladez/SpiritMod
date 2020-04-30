using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodMoon
{
    [AutoloadBossHead]
    public class Occultist : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occultist");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 60;

			npc.lifeMax = 431;
			npc.defense = 14;
			npc.damage = 30;

			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;

			npc.value = 300f;
			npc.knockBackResist = 0.45f;
			npc.netAlways = true;
			npc.lavaImmune = true;
		}
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.12f, .64f);

			npc.TargetClosest(true);
			
			//"teleport away" at daylight
			if (Main.dayTime)
			{
				npc.alpha = 255;
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
				npc.active = false;
				npc.life = 0;
			}
			if (npc.ai[2] != 0 && npc.ai[3] != 0)
			{
				// Teleport effects: away.
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0.0f, 0.0f, 100, new Color(), 1.5f);
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
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
			}
			npc.velocity.X = npc.velocity.X * 0.93f;
			if (npc.velocity.X > -0.1F && npc.velocity.X < 0.1F)
				npc.velocity.X = 0;
			++npc.ai[0];

			if (npc.ai[0] == 300)
			{
				npc.ai[1] = 40f;
				npc.netUpdate = true;
			}

			bool teleport = false;

			// Teleport
			if (npc.ai[0] >= 600 && Main.netMode != 1)
			{
				teleport = true;
			}

			if (teleport)
				Teleport();

			if (npc.ai[1] > 0)
			{
				--npc.ai[1];
                if (npc.ai[1] == 0)
                {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                    if (Main.netMode != 1)
                    {
                        Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
                        if (Main.rand.Next(4) == 0)
                        {
                            {
                                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                                direction.Normalize();
                                direction.X *= 6f;
                                direction.Y *= 6f;
                                float A = (float)Main.rand.Next(-120, 120) * 0.01f;
                                float B = (float)Main.rand.Next(-120, 120) * 0.01f;
                                for (int z = 0; z <= Main.rand.Next(1, 4); z++)
                                {
                                    int p = Projectile.NewProjectile((int)npc.position.X + Main.rand.Next(-60, 60), (int)npc.position.Y + Main.rand.Next(-200, -100), direction.X + A, direction.Y + B, mod.ProjectileType("OccultistHand"), 16, 1, Main.myPlayer, 0, 0);
                                    Main.projectile[p].velocity.X = Main.player[npc.target].Center.X - Main.projectile[p].Center.X;
                                    Main.projectile[p].velocity.Y = Main.player[npc.target].Center.Y - Main.projectile[p].Center.Y;
                                    Main.projectile[p].velocity.Normalize();
                                    Main.projectile[p].velocity.X *= 3;
                                    Main.projectile[p].velocity.Y *= 3;
                                    Main.PlaySound(2, (int)Main.projectile[p].position.X, (int)Main.projectile[p].position.Y, 8);
                                    for (int i = 0; i < 10; i++)
                                    {
                                        int num = Dust.NewDust(Main.projectile[p].position, Main.projectile[p].width, Main.projectile[p].height, 173, 0f, -2f, 0, default(Color), 2f);
                                        Main.dust[num].noGravity = true;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        if (Main.dust[num].position != Main.projectile[p].Center)
                                        {
                                            Main.dust[num].velocity = Main.projectile[p].DirectionTo(Main.dust[num].position) * 6f;
                                        }
                                    }
                                }
                            }
                        }
                        if (Main.rand.Next(8) == 0 || (!NPC.AnyNPCs(NPCID.BloodZombie)))
                        {
                            for (int z = 0; z <= Main.rand.Next(1, 4); z++)
                            {
                                if (Main.rand.Next(4) == 0)
                                {
                                    int p = NPC.NewNPC((int)Main.player[npc.target].position.X + Main.rand.Next(-200, 200), (int)Main.player[npc.target].position.Y + Main.rand.Next(-200, -100), NPCID.BloodZombie, 0, 0, 0, 0, 0, 255);
                                    for (int i = 0; i < 10; i++)
                                    {
                                        int num = Dust.NewDust(Main.npc[p].position, Main.npc[p].width, Main.npc[p].height, 173, 0f, -2f, 0, default(Color), 2f);
                                        Main.dust[num].noGravity = true;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        if (Main.dust[num].position != Main.npc[p].Center)
                                        {
                                            Main.dust[num].velocity = Main.npc[p].DirectionTo(Main.dust[num].position) * 6f;
                                        }
                                    }
                                }
                                else
                                {
                                    int p = NPC.NewNPC((int)Main.player[npc.target].position.X + Main.rand.Next(-200, 200), (int)Main.player[npc.target].position.Y + Main.rand.Next(-200, -100), NPCID.Zombie, 0, 0, 0, 0, 0, 255);
                                    for (int i = 0; i < 10; i++)
                                    {
                                        int num = Dust.NewDust(Main.npc[p].position, Main.npc[p].width, Main.npc[p].height, 173, 0f, -2f, 0, default(Color), 2f);
                                        Main.dust[num].noGravity = true;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                        if (Main.dust[num].position != Main.npc[p].Center)
                                        {
                                            Main.dust[num].velocity = Main.npc[p].DirectionTo(Main.dust[num].position) * 6f;
                                        }
                                    }
                                }
                            }
                        }
                        else if (NPC.AnyNPCs(NPCID.BloodZombie))
                        {
                            int feast = NPC.FindFirstNPC(NPCID.BloodZombie);
                            Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 6);
                            Main.PlaySound(4, Main.npc[feast].position, 2);
                            Main.npc[feast].life = 0;
                            for (int i = 0; i < 40; i++)
                            {
                                int num = Dust.NewDust(Main.npc[feast].position, Main.npc[feast].width, Main.npc[feast].height, 173, 0f, -2f, 0, default(Color), 2f);
                                Main.dust[num].noGravity = true;
                                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                if (Main.dust[num].position != Main.npc[feast].Center)
                                {
                                    Main.dust[num].velocity = Main.npc[feast].DirectionTo(Main.dust[num].position) * 6f;
                                }
                            }
                            if (npc.life <= npc.lifeMax - 30)
                            {
                                npc.life += 30;
                                npc.HealEffect(30, true);
                            }
                            else if (npc.life < npc.lifeMax)
                            {
                                npc.HealEffect(npc.lifeMax - npc.life, true);
                                npc.life += npc.lifeMax - npc.life;
                            }
                        }
					}
				}
			}

			if (Main.rand.Next(3) == 0)
				return false;
			Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 173, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
			dust.noGravity = true;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BloodMoon/Occultist_Glow"));
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.75f);
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
                            npc.ai[1] = 30;
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
        
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && !NPC.AnyNPCs(mod.NPCType("Occultist")) && NPC.downedBoss1 ? 0.04f : 0f;
		}
        public override void NPCLoot()
        {
            string[] lootTable = { "Handball", "OccultistStaff" };
            int loot = Main.rand.Next(lootTable.Length);
            {
                npc.DropItem(mod.ItemType(lootTable[loot]));
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BloodFire"), 4 + Main.rand.Next(3, 5));
        }
        public override void HitEffect(int hitDirection, double damage)
		{
				int d = 173;
				int d1 = 173;
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0,default(Color), 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist4"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist5"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist6"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Occultist/Occultist7"));
                for (int k = 0; k < 60; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -4.5f, 0, default(Color), Main.rand.NextFloat(.9f, 1.4f));
                    Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -4.5f, 0, default(Color), Main.rand.NextFloat(.9f, 1.4f));
                }
            }
		}
	}
}
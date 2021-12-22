using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Masticator
{
	public class Masticator : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masticator");
			Main.npcFrameCount[npc.type] = 11;
			NPCID.Sets.TrailCacheLength[npc.type] = 2;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.damage = 24;
			npc.width = 36; //324
			npc.height = 42; //216
			npc.defense = 6;
			npc.lifeMax = 80;
			npc.knockBackResist = 0.75f;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 0, 80);
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath49;
			npc.buffImmune[BuffID.Confused] = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.MasticatorBanner>();
		}
		int frame;
		bool vomitPhase;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(vomitPhase);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			vomitPhase = reader.ReadBoolean();
		}
		public override void AI()
		{
			if (!vomitPhase) {
				npc.rotation = npc.velocity.X * .06f;
				++npc.ai[2];
				if (npc.ai[2] >= 6) {
					frame++;
					npc.ai[2] = 0;
				}
				if (frame >= 4) {
					frame = 0;
				}
				npc.TargetClosest(true);
				float num1164 = 4f;
				float num1165 = 0.35f;
				Vector2 vector133 = new Vector2(npc.Center.X, npc.Center.Y);
				float num1166 = Main.player[npc.target].Center.X - vector133.X;
				float num1167 = Main.player[npc.target].Center.Y - vector133.Y - 120f;
				float num1168 = (float)Math.Sqrt((double)(num1166 * num1166 + num1167 * num1167));
				if (num1168 < 6f) {
					num1166 = npc.velocity.X;
					num1167 = npc.velocity.Y;
				}
				else {
					num1168 = num1164 / num1168;
					num1166 *= num1168;
					num1167 *= num1168;
				}
				if (npc.velocity.X < num1166) {
					npc.velocity.X = npc.velocity.X + num1165;
					if (npc.velocity.X < 0f && num1166 > 0f) {
						npc.velocity.X = npc.velocity.X + num1165 * .35f;
					}
                }
				else if (npc.velocity.X > num1166) {
					npc.velocity.X = npc.velocity.X - num1165;
					if (npc.velocity.X > 0f && num1166 < 0f) {
						npc.velocity.X = npc.velocity.X - num1165 * .35f;
					}
                }
				if (npc.velocity.Y < num1167) {
					npc.velocity.Y = npc.velocity.Y + num1165;
					if (npc.velocity.Y < 0f && num1167 > 0f) {
						npc.velocity.Y = npc.velocity.Y + num1165 * .35f;
					}
                }
				else if (npc.velocity.Y > num1167) {
					npc.velocity.Y = npc.velocity.Y - num1165;
					if (npc.velocity.Y > 0f && num1167 < 0f) {
						npc.velocity.Y = npc.velocity.Y - num1165 * .35f;
					}
                }
				if (npc.position.X + (float)npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != NetmodeID.MultiplayerClient) {
					npc.ai[0] += 1f;
                    npc.rotation = 0f;
                    if (npc.ai[0] > 90f) {
						vomitPhase = true;
                        npc.netUpdate = true;
                    }
				}
			}
			else {
				if (Main.rand.NextFloat() < 0.331579f) {
					{
						Vector2 position = npc.Center;
						int d = Dust.NewDust(npc.position, npc.width, npc.height + 10, DustID.Plantera_Green, 0, 1f, 0, Color.Purple, 0.7f);
						Main.dust[d].velocity *= .1f;
					}
				}
                npc.rotation = 0f;
				npc.velocity.X = .001f * npc.direction;
				npc.velocity.Y = 0f;
				++npc.ai[3];
				if (npc.ai[3] >= 210) {
					npc.ai[3] = 0;
                    npc.netUpdate = true;
					vomitPhase = false;
					npc.netUpdate = true;
				}

				if (npc.ai[3] > 50 && npc.ai[3] < 180) {
					++npc.ai[2];
					if (npc.ai[2] >= 6) {
						frame++;
						npc.ai[2] = 0;
					}
					if (frame >= 10) {
						frame = 5;
					}
					npc.rotation = 0f;
					float num395 = Main.mouseTextColor / 200f - 0.25f;
					num395 *= 0.2f;
					npc.scale = num395 + 0.95f;
					if (Main.rand.NextBool(12) && Main.netMode != NetmodeID.MultiplayerClient) {
						npc.velocity.Y -= .1f;
						bool expertMode = Main.expertMode;
						int damage = expertMode ? 12 : 15;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 4, Main.rand.NextFloat(-.85f, .85f), Main.rand.NextFloat(4f, 6f), ModContent.ProjectileType<CorruptVomitProj>(), damage, 1, Main.myPlayer, 0, 0);
						npc.netUpdate = true;
					}
                    if (Main.rand.NextBool(16)) {

                        int tomaProj;
                        tomaProj = Main.rand.Next(new int[] { mod.ProjectileType("Teratoma1"), mod.ProjectileType("Teratoma2"), mod.ProjectileType("Teratoma3") });
                        bool expertMode = Main.expertMode;
                        Main.PlaySound(SoundID.Item20, npc.Center);
                        int damagenumber = expertMode ? 12 : 17;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 6, Main.rand.Next(-3, 3), Main.rand.NextFloat(1f, 3f), tomaProj, damagenumber, 1, Main.myPlayer, 0, 0);
                            Main.projectile[p].penetrate = 1;
                        }
					}
				}
				else {
					++npc.ai[2];
					if (npc.ai[2] >= 6) {
						frame++;
						npc.ai[2] = 0;
					}
					if (frame >= 4) {
						frame = 0;
					}
				}
				if (npc.ai[3] == 180) {
					Main.PlaySound(SoundID.NPCKilled, npc.Center, 13);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCorrupt && spawnInfo.player.ZoneOverworldHeight && NPC.CountNPCS(ModContent.NPCType<Masticator>()) < 2 ? .2f : 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 22;
			int d1 = 184;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma1"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma2"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma3"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma4"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma5"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma6"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma7"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma5"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma6"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma7"), Main.rand.NextFloat(.85f, 1.1f));
			}
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RottenChunk);
            }
            if (Main.rand.NextBool(5))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WormTooth);
            }
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frame * frameHeight;
		}
	}
}

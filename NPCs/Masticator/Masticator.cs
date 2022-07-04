using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Putroma;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Masticator
{
	public class Masticator : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masticator");
			Main.npcFrameCount[NPC.type] = 11;
			NPCID.Sets.TrailCacheLength[NPC.type] = 2;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.damage = 24;
			NPC.width = 36; //324
			NPC.height = 42; //216
			NPC.defense = 6;
			NPC.lifeMax = 80;
			NPC.knockBackResist = 0.75f;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 0, 80);
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath49;
			NPC.buffImmune[BuffID.Confused] = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MasticatorBanner>();
		}

		int frame;
		bool vomitPhase;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(vomitPhase);
		public override void ReceiveExtraAI(BinaryReader reader) => vomitPhase = reader.ReadBoolean();

		public override void AI()
		{
			if (!vomitPhase)
			{
				NPC.rotation = NPC.velocity.X * .06f;
				++NPC.ai[2];
				if (NPC.ai[2] >= 6)
				{
					frame++;
					NPC.ai[2] = 0;
				}

				if (frame >= 4)
					frame = 0;

				NPC.TargetClosest(true);
				float num1164 = 4f;
				float num1165 = 0.35f;
				Vector2 vector133 = new Vector2(NPC.Center.X, NPC.Center.Y);
				float num1166 = Main.player[NPC.target].Center.X - vector133.X;
				float num1167 = Main.player[NPC.target].Center.Y - vector133.Y - 120f;
				float num1168 = (float)Math.Sqrt((double)(num1166 * num1166 + num1167 * num1167));

				if (num1168 < 6f)
				{
					num1166 = NPC.velocity.X;
					num1167 = NPC.velocity.Y;
				}
				else
				{
					num1168 = num1164 / num1168;
					num1166 *= num1168;
					num1167 *= num1168;
				}

				if (NPC.velocity.X < num1166)
				{
					NPC.velocity.X = NPC.velocity.X + num1165;
					if (NPC.velocity.X < 0f && num1166 > 0f)
						NPC.velocity.X = NPC.velocity.X + num1165 * .35f;
				}
				else if (NPC.velocity.X > num1166)
				{
					NPC.velocity.X = NPC.velocity.X - num1165;
					if (NPC.velocity.X > 0f && num1166 < 0f)
						NPC.velocity.X = NPC.velocity.X - num1165 * .35f;
				}

				if (NPC.velocity.Y < num1167)
				{
					NPC.velocity.Y = NPC.velocity.Y + num1165;
					if (NPC.velocity.Y < 0f && num1167 > 0f)
						NPC.velocity.Y = NPC.velocity.Y + num1165 * .35f;
				}
				else if (NPC.velocity.Y > num1167)
				{
					NPC.velocity.Y = NPC.velocity.Y - num1165;

					if (NPC.velocity.Y > 0f && num1167 < 0f)
						NPC.velocity.Y = NPC.velocity.Y - num1165 * .35f;
				}

				if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[0] += 1f;
					NPC.rotation = 0f;
					if (NPC.ai[0] > 90f)
					{
						vomitPhase = true;
						NPC.netUpdate = true;
					}
				}
			}
			else
			{
				if (Main.rand.NextFloat() < 0.331579f)
				{
					Vector2 position = NPC.Center;
					int d = Dust.NewDust(NPC.position, NPC.width, NPC.height + 10, DustID.Plantera_Green, 0, 1f, 0, Color.Purple, 0.7f);
					Main.dust[d].velocity *= .1f;
				}
				NPC.rotation = 0f;
				NPC.velocity.X = .001f * NPC.direction;
				NPC.velocity.Y = 0f;
				++NPC.ai[3];
				if (NPC.ai[3] >= 210)
				{
					NPC.ai[3] = 0;
					NPC.netUpdate = true;
					vomitPhase = false;
					NPC.netUpdate = true;
				}

				if (NPC.ai[3] > 50 && NPC.ai[3] < 180)
				{
					++NPC.ai[2];
					if (NPC.ai[2] >= 6)
					{
						frame++;
						NPC.ai[2] = 0;
					}

					if (frame >= 10)
						frame = 5;

					NPC.rotation = 0f;
					float num395 = Main.mouseTextColor / 200f - 0.25f;
					num395 *= 0.2f;
					NPC.scale = num395 + 0.95f;

					if (Main.rand.NextBool(12) && Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.velocity.Y -= .1f;
						bool expertMode = Main.expertMode;
						int damage = expertMode ? 12 : 15;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y + 4, Main.rand.NextFloat(-.85f, .85f), Main.rand.NextFloat(4f, 6f), ModContent.ProjectileType<CorruptVomitProj>(), damage, 1, Main.myPlayer, 0, 0);
						NPC.netUpdate = true;
					}

					if (Main.rand.NextBool(16))
					{

						int tomaProj;
						tomaProj = Main.rand.Next(new int[] { ModContent.ProjectileType<Teratoma1>(), ModContent.ProjectileType<Teratoma2>(), ModContent.ProjectileType<Teratoma3>() });
						bool expertMode = Main.expertMode;
						SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
						int damagenumber = expertMode ? 12 : 17;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y + 6, Main.rand.Next(-3, 3), Main.rand.NextFloat(1f, 3f), tomaProj, damagenumber, 1, Main.myPlayer, 0, 0);
							Main.projectile[p].penetrate = 1;
						}
					}
				}
				else
				{
					++NPC.ai[2];
					if (NPC.ai[2] >= 6)
					{
						frame++;
						NPC.ai[2] = 0;
					}
					if (frame >= 4)
						frame = 0;
				}

				if (NPC.ai[3] == 180)
					SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight && NPC.CountNPCS(ModContent.NPCType<Masticator>()) < 2 ? .2f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pot, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, Color.White, .34f);
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma1").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma2").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma3").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma4").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma5").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma6").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma7").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma5").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma6").Type, Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Teratoma7").Type, Main.rand.NextFloat(.85f, 1.1f));
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.WormTooth, 5));
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frame * frameHeight;
	}
}
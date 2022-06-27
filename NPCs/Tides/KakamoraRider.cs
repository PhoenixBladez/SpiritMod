using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraRider : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamoran Rider");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 38;
			NPC.damage = 18;
			NPC.defense = 4;
			AIType = NPCID.SnowFlinx;
			NPC.aiStyle = 3;
			NPC.lifeMax = 160;
			NPC.knockBackResist = .70f;
			NPC.value = 200f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.KakamoraBanner>();
		}

		// localai0 : 0 when spawned, 1 when otherNPC spawned. 
		// ai0 = npc number of other NPC
		// ai1 = charge time for gun.
		// ai2 = used for frame??
		bool checkSpawn = false;
		public override void AI()
		{
			int otherNPC = -1;
			Vector2 offsetFromOtherNPC = new Vector2(-15, -18);
			if (NPC.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient && !checkSpawn)
			{

				NPC.localAI[0] = 1f;
				int newNPC = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Crocomount>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
				NPC.ai[0] = (float)newNPC;
				checkSpawn = true;
				NPC.netUpdate = true;
			}

			int otherNPCCheck = (int)NPC.ai[0];

			if (Main.npc[otherNPCCheck].active && Main.npc[otherNPCCheck].type == ModContent.NPCType<Crocomount>())
			{
				if (NPC.timeLeft < 60)
					NPC.timeLeft = 60;
				otherNPC = otherNPCCheck;
			}

			// If otherNPC exists, do this
			if (otherNPC != -1)
			{
				NPC nPC7 = Main.npc[otherNPC];
				NPC.velocity = Vector2.Zero;
				NPC.position = nPC7.Center;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				NPC.position.Y += offsetFromOtherNPC.Y;
				NPC.position.X += offsetFromOtherNPC.X * nPC7.direction;
				NPC.gfxOffY = nPC7.gfxOffY;
				NPC.rotation = 0f;
				NPC.direction = nPC7.direction;
				NPC.spriteDirection = nPC7.spriteDirection;
				NPC.timeLeft = nPC7.timeLeft;
				NPC.velocity = nPC7.velocity;
				NPC.target = nPC7.target;

				if (NPC.ai[1] < 60f)
					NPC.ai[1] += 1f;
				if (NPC.justHit)
					NPC.ai[1] = -30f;

				int projectileType = ProjectileID.RayGunnerLaser;// 438;
				int projectileDamage = 30;
				float scaleFactor20 = 7f;
				if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					Vector2 vectorToPlayer = Main.player[NPC.target].Center - NPC.Center;
					Vector2 vectorToPlayerNormalized = Vector2.Normalize(vectorToPlayer);
					float num1547 = vectorToPlayer.Length();
					float num1548 = 700f;

					if (num1547 < num1548)
					{
						if (NPC.ai[1] == 60f && Math.Sign(vectorToPlayer.X) == NPC.direction)
						{
							NPC.ai[1] = -60f;
							Vector2 center12 = Main.player[NPC.target].Center;
							Vector2 value26 = NPC.Center - Vector2.UnitY * 4f;
							Vector2 vector188 = center12 - value26;
							vector188.X += (float)Main.rand.Next(-50, 51);
							vector188.Y += (float)Main.rand.Next(-50, 51);
							vector188.X *= (float)Main.rand.Next(80, 121) * 0.01f;
							vector188.Y *= (float)Main.rand.Next(80, 121) * 0.01f;
							vector188.Normalize();
							if (float.IsNaN(vector188.X) || float.IsNaN(vector188.Y))
								vector188 = -Vector2.UnitY;

							vector188 *= scaleFactor20;
							Projectile.NewProjectile(value26.X, value26.Y, vector188.X, vector188.Y, projectileType, projectileDamage, 0f, Main.myPlayer, 0f, 0f);
							NPC.netUpdate = true;
						}
						else
						{
							float oldAI2 = NPC.ai[2];
							NPC.velocity.X = NPC.velocity.X * 0.5f;
							NPC.ai[2] = 3f;

							if (Math.Abs(vectorToPlayerNormalized.Y) > Math.Abs(vectorToPlayerNormalized.X) * 2f)
							{
								if (vectorToPlayerNormalized.Y > 0f)
									NPC.ai[2] = 1f;
								else
									NPC.ai[2] = 5f;
							}
							else if (Math.Abs(vectorToPlayerNormalized.X) > Math.Abs(vectorToPlayerNormalized.Y) * 2f)
								NPC.ai[2] = 3f;
							else if (vectorToPlayerNormalized.Y > 0f)
								NPC.ai[2] = 2f;
							else
								NPC.ai[2] = 4f;

							if (NPC.ai[2] != oldAI2)
								NPC.netUpdate = true;
						}
					}
				}
			}
			else
			{
				// This code is called when Bottom is dead. Top is transformed into a new NPC.
				switch (Main.rand.Next(4))
				{
					case 0:
						NPC.Transform(ModContent.NPCType<KakamoraRunner>());
						break;
					case 1:
						NPC.Transform(ModContent.NPCType<SpearKakamora>());
						break;
					case 2:
						NPC.Transform(ModContent.NPCType<SwordKakamora>());
						break;
					case 3:
						NPC.Transform(ModContent.NPCType<KakamoraShielder>());
						break;
				}
				return;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
				}

				SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));

				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore3").Type, 1f);
			}
		}
	}
}

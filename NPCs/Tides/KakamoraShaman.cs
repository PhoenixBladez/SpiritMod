﻿
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Tides.Tide;
using SpiritMod.Items.Sets.TideDrops;


namespace SpiritMod.NPCs.Tides
{
	public class KakamoraShaman : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora Shaman");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 58;
			NPC.height = 54;
			NPC.damage = 24;
			NPC.defense = 14;
			AIType = NPCID.SnowFlinx;
			NPC.aiStyle = 3;
			NPC.lifeMax = 180;
			NPC.knockBackResist = 0.1f;
			NPC.value = 400f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.KakamoraShamanBanner>();
		}
		bool blocking = false;
		int blockTimer = 0;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (NPC.wet)
			{
				NPC.noGravity = true;
				if (NPC.velocity.Y > -7)
				{
					NPC.velocity.Y -= .085f;
				}
				return;
			}
			else
			{
				NPC.noGravity = false;
			}
			blockTimer++;
			if (blockTimer == 200)
			{
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);
				NPC.frameCounter = 0;
				healed = false;
				NPC.velocity.X = 0;
			}
			if (blockTimer > 200)
			{
				blocking = true;
			}
			if (blockTimer > 350)
			{
				blocking = false;
				blockTimer = 0;
				NPC.frameCounter = 0;
			}
			if (blocking)
			{
				NPC.aiStyle = 0;
				NPC.noGravity = false;
				if (player.position.X > NPC.position.X)
				{
					NPC.spriteDirection = 1;
				}
				else
				{
					NPC.spriteDirection = -1;
				}
			}
			else
			{
				NPC.spriteDirection = NPC.direction;
				NPC.aiStyle = 3;
				var list = Main.npc.Where(x => x.Hitbox.Intersects(NPC.Hitbox));
				foreach (var npc2 in list)
				{
					if (npc2.type == ModContent.NPCType<LargeCrustecean>() && NPC.Center.Y > npc2.Center.Y && npc2.active)
					{
						NPC.velocity.X = npc2.direction * 7;
						NPC.velocity.Y = -2;
						SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
					}
				}
			}
		}
		public override void OnKill()
		{
			if (Main.rand.NextBool(50))
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<CoconutGun>());
			}
			if (Main.rand.NextBool(50))
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<TikiJavelin>());
			}
			if (Main.rand.NextBool(15))
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<MagicConch>());
			}
			if (Main.rand.Next(3) != 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<TribalScale>(), Main.rand.Next(2) + 2);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if ((NPC.collideY || NPC.wet) && !blocking)
			{
				NPC.frameCounter += 0.2f;
				NPC.frameCounter %= 4;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
			if (NPC.wet)
			{
				return;
			}
			if (blocking)
			{
				NPC.frameCounter += 0.05f;
				if (NPC.frameCounter > 2 && !healed)
				{
					var list = Main.npc;
					foreach (var npc2 in list)
					{
						if (npc2.type == ModContent.NPCType<KakamoraRunner>() || npc2.type == ModContent.NPCType<KakamoraShielder>() || npc2.type == ModContent.NPCType<KakamoraShielderRare>() || npc2.type == ModContent.NPCType<SpearKakamora>() || npc2.type == ModContent.NPCType<SwordKakamora>())
						{
							if (Math.Abs(npc2.position.X - NPC.position.X) < 500 && npc2.active && npc2.life < npc2.lifeMax) //500 is distance away he heals
							{
								int bolt = Terraria.Projectile.NewProjectile(NPC.Center.X + Main.rand.Next(-66, 66), NPC.Center.Y - Main.rand.Next(60, 120), 0, 0, ModContent.ProjectileType<ShamanBolt>(), 0, 0);
								Projectile p = Main.projectile[bolt];
								Vector2 direction = npc2.Center - p.Center;
								direction.Normalize();
								direction *= 4;
								p.velocity = direction;
								p.ai[0] = direction.X;
								p.ai[1] = direction.Y;
							}
						}
					}

					for (int j = 0; j < 25; j++)
						Dust.NewDustPerfect(new Vector2(NPC.Center.X + NPC.direction * 22, NPC.Center.Y), 173, new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-16, 0)));

					healed = true;
				}

				NPC.frameCounter = MathHelper.Clamp((float)NPC.frameCounter, 0, 2.9f);
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = (frame + 4) * frameHeight;
			}
		}

		bool healed = false;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore3").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/ShamanGore1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/ShamanGore2").Type, 1f);
			}
		}
	}
}
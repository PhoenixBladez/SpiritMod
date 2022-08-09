using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraShaman : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora Shaman");
			Main.npcFrameCount[NPC.type] = 7;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
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

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Pint-sized mystics that attack using the arcane arts. They double as healers for their nearby Kakamora companions."),
			});
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
					NPC.velocity.Y -= .085f;
				return;
			}
			else
				NPC.noGravity = false;

			blockTimer++;
			if (blockTimer == 200)
			{
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);
				NPC.frameCounter = 0;
				healed = false;
				NPC.velocity.X = 0;
			}

			if (blockTimer > 200)
				blocking = true;

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
					NPC.spriteDirection = 1;
				else
					NPC.spriteDirection = -1;
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
						SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraHit"), NPC.Center);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<CoconutGun>(50);
			npcLoot.AddCommon<TikiJavelin>(50);
			npcLoot.AddCommon<MagicConch>(15);
			npcLoot.AddCommon<TribalScale>(3, 2, 3);
		}

		public override void FindFrame(int frameHeight)
		{
			if (((NPC.collideY || NPC.wet) && !blocking) || NPC.IsABestiaryIconDummy)
			{
				NPC.frameCounter += 0.2f;
				NPC.frameCounter %= 4;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}

			if (NPC.wet)
				return;

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
								int bolt = Terraria.Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-66, 66), NPC.Center.Y - Main.rand.Next(60, 120), 0, 0, ModContent.ProjectileType<ShamanBolt>(), 0, 0);
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
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraDeath"), NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_Gore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_Gore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ShamanGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ShamanGore2").Type, 1f);
			}
		}
	}
}
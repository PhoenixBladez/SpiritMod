using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.GraniteSet;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.GraniteSlime
{
	public class GraniteSlime : ModNPC
	{
		bool jump;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 12;
			NPC.damage = 22;
			NPC.defense = 8;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.lifeMax = 85;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath43;
			NPC.value = 200f;
			NPC.knockBackResist = .25f;
			NPC.aiStyle = 1;
			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.GraniteSlimeBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
				new FlavorTextBestiaryInfoElement("A gelatinous creature that’s roamed the Granite caves for a long while, accumulating enough pebbles and dust to become one with the stone."),
			});
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, ModContent.Request<Texture2D>("SpiritMod/NPCs/GraniteSlime/GraniteSlime_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, screenPos);

		public override void AI()
		{
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			bool expertMode = Main.expertMode;
			NPC.direction = NPC.spriteDirection;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.12f, 0.29f, .42f);

			if (!NPC.collideY)
				jump = true;

			if (NPC.collideY && jump && Main.rand.Next(3) == 0)
			{
				for (int i = 0; i < 20; i++)
				{
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
				}
				jump = false;
				SoundEngine.PlaySound(SoundID.Item110, NPC.Center);
				int damage = expertMode ? 11 : 21;
				if (distance < 92)
				{
					target.AddBuff(BuffID.Confused, 180);
					target.AddBuff(BuffID.Shine, 180);
				}
				for (int i = 0; i < 2; i++)
				{
					float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
					Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<GraniteShard1>(), 13, 1, Main.myPlayer, 0, 0);
					Main.projectile[proj].friendly = false;
					Main.projectile[proj].hostile = true;
					Main.projectile[proj].velocity *= 4f;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 2, 3));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteChunk>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemID.NightVisionHelmet, 1000));
			npcLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, 10000));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.SpawnTileType == 368) && NPC.downedBoss2 && spawnInfo.SpawnTileY > Main.rockLayer ? 0.17f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.87f);
				}
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0 && Main.rand.Next(3) == 0)
			{
				SoundEngine.PlaySound(SoundID.Item109);
				for (int i = 0; i < 20; i++)
				{
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
				}

				Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
				NPC.NewNPC(NPC.GetSource_Death(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<NPCs.CracklingCore.GraniteCore>());
			}
		}
	}
}
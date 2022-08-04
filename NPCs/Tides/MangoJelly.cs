using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Tides
{
	public class MangoJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mang-O War");
			Main.npcFrameCount[NPC.type] = 8;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 50;
			NPC.damage = 30;
			NPC.defense = 6;
			NPC.lifeMax = 225;
			NPC.noGravity = true;
			NPC.knockBackResist = .03f;
			NPC.value = 200f;
			NPC.alpha = 35;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MangoWarBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Small, airborne marine life. While named after their fruity smell and vibrant color, they are not in fact sweet."),
			});
		}

		int xoffset = 0;
		bool createdLaser = false;
		float bloomCounter = 1;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[3] == 1)
				Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, (NPC.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(22.5f * bloomCounter), (int)(13.8f * bloomCounter), (int)(21.6f * bloomCounter), 0), 0f, new Vector2(50, 50), 0.125f * (bloomCounter + 3), SpriteEffects.None, 0f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);

			if (NPC.life <= 0)
				for (int i = 1; i < 7; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MangoJelly" + i).Type, 1f);
		}

		public override void AI()
		{
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
			NPC.ai[0]++;

			if (player.position.X > NPC.position.X)
				xoffset = 24;
			else
				xoffset = -24;

			if (NPC.ai[0] == 400 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[2] = 1;
				createdLaser = false;
				NPC.frameCounter = 0;
				NPC.netUpdate = true;
			}

			if (NPC.ai[0] == 550)
			{
				bloomCounter = 1;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[3] = 0;
					Vector2 vel = new Vector2(30f, 0).RotatedBy((float)(Main.rand.Next(90) * Math.PI / 180));
					SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
					for (int i = 0; i < 4; i++)
					{
						int lozar = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + vel.RotatedBy(i * 1.57f) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), NPC.damage / 3, 0, Main.myPlayer);
						Main.projectile[lozar].netUpdate = true;
					}
					NPC.netUpdate = true;
				}
			}

			if (NPC.ai[0] >= 570 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[2] = 0;
				NPC.ai[0] = 0;
				NPC.netUpdate = true;
			}

			if (NPC.ai[2] == 1)
			{ //shooting
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				NPC.scale = num395 + 0.95f;
				NPC.knockBackResist = 0;
				NPC.velocity = Vector2.Zero;
				bloomCounter += 0.02f;
				NPC.rotation = 0f;
			}
			else
			{
				NPC.knockBackResist = .9f;
				#region regular movement
				NPC.velocity.X *= 0.99f;
				if (NPC.ai[1] == 0)
				{ //not jumping
					if (NPC.velocity.Y < 2.5f)
					{
						NPC.velocity.Y += 0.1f;
					}
					if (player.position.Y < NPC.position.Y && NPC.ai[0] % 30 == 0)
					{
						NPC.ai[1] = 1;
						NPC.velocity.X = xoffset / 1.25f;
						NPC.velocity.Y = -6;
					}
				}
				if (NPC.ai[1] == 1)
				{ //jumping
					NPC.velocity *= 0.97f;
					if (Math.Abs(NPC.velocity.X) < 0.125f)
					{
						NPC.ai[1] = 0;
					}
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
				}
				#endregion
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<MagicConch>(25);
			npcLoot.AddCommon<MangoJellyStaff>(25);
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];
			if (NPC.ai[2] == 0)
			{
				if (player.position.Y < NPC.position.Y)
					NPC.frameCounter += 0.10f;

				NPC.frameCounter += 0.05f;
				NPC.frameCounter %= 4;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
			else
			{
				if (NPC.frameCounter < 2.8f)
					NPC.frameCounter += 0.1f;
				else if (NPC.ai[3] == 0 && NPC.frameCounter < 3.8f)
					NPC.frameCounter += 0.08f;

				if (NPC.frameCounter >= 2.8f && !createdLaser)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
						NPC.ai[3] = 1;
					createdLaser = true;
					NPC.netUpdate = true;
				}
				NPC.frameCounter %= 4;
				int frame = (int)NPC.frameCounter + 4;
				NPC.frame.Y = frame * frameHeight;
			}
		}
	}
}
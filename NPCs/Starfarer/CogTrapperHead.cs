using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.ProtectorateSet;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using SpiritMod.Biomes;

namespace SpiritMod.NPCs.Starfarer
{
	public class CogTrapperHead : ModNPC
	{
		public bool flies = true;
		public bool directional = false;
		public float speed = 6.5f;
		public float turnSpeed = 0.125f;
		public bool tail = false;
		public int minLength = 15;
		public int maxLength = 16;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardancer");
			Main.npcFrameCount[NPC.type] = 1; //new
		}

		public override void SetDefaults()
		{
			NPC.damage = 32; //150
			NPC.npcSlots = 17f;
			NPC.width = 26; //324
			NPC.height = 26; //216
			NPC.defense = 0;
			NPC.lifeMax = 225; //250000
			NPC.aiStyle = 6; //new
			AIType = -1; //new
			AnimationType = 10; //new
			NPC.knockBackResist = 0f;
			NPC.value = 540;
			NPC.alpha = 255;
			NPC.behindTiles = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.StardancerBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<AsteroidBiome>().Type };

			for (int k = 0; k < NPC.buffImmune.Length; k++) {
				NPC.buffImmune[k] = true;
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Strange mechanical constructs assemble around a beacon, calling attention to precious astral metals. It seems you aren’t the only one who wants it."),
			});
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0f, 0.0375f * 2, 0.125f * 2);
			if (NPC.ai[3] > 0f)
				NPC.realLife = (int)NPC.ai[3];

			if (NPC.target < 0 || NPC.target == 255 || player.dead)
				NPC.TargetClosest(true);

			if (NPC.alpha != 0) {
				for (int num934 = 0; num934 < 2; num934++) {
					int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 1f);
					Main.dust[num935].noGravity = true;
					Main.dust[num935].noLight = true;
				}
			}
			NPC.alpha -= 12;
			if (NPC.alpha < 0)
				NPC.alpha = 0;

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (!tail && NPC.ai[0] == 0f) {
					int current = NPC.whoAmI;
					for (int num36 = 0; num36 < maxLength; num36++) {
						int trailing = 0;
						if (num36 >= 0 && num36 < minLength)
							trailing = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<CogTrapperBody>(), NPC.whoAmI);
						else
							trailing = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<CogTrapperTail>(), NPC.whoAmI);
						Main.npc[trailing].realLife = NPC.whoAmI;
						Main.npc[trailing].ai[2] = (float)NPC.whoAmI;
						Main.npc[trailing].ai[1] = (float)current;
						Main.npc[current].ai[0] = (float)trailing;
						NPC.netUpdate = true;
						current = trailing;
					}
					tail = true;
				}

				if (!NPC.active && Main.netMode == NetmodeID.Server) {
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}
			}

			int num180 = (int)(NPC.position.X / 16f) - 1;
			int num181 = (int)((NPC.position.X + (float)NPC.width) / 16f) + 2;
			int num182 = (int)(NPC.position.Y / 16f) - 1;
			int num183 = (int)((NPC.position.Y + (float)NPC.height) / 16f) + 2;

			if (num180 < 0)
				num180 = 0;
			if (num181 > Main.maxTilesX)
				num181 = Main.maxTilesX;
			if (num182 < 0)
				num182 = 0;
			if (num183 > Main.maxTilesY)
				num183 = Main.maxTilesY;

			bool flag94 = flies;
			NPC.localAI[1] = 0f;
			if (directional) {
				if (NPC.velocity.X < 0f)
					NPC.spriteDirection = 1;
				else if (NPC.velocity.X > 0f)
					NPC.spriteDirection = -1;
			}

			if (player.dead) {
				NPC.TargetClosest(false);
				flag94 = false;
				NPC.velocity.Y = NPC.velocity.Y + 10f;
				if ((double)NPC.position.Y > Main.worldSurface * 16.0)
					NPC.velocity.Y = NPC.velocity.Y + 10f;
				if ((double)NPC.position.Y > Main.rockLayer * 16.0) {
					for (int num957 = 0; num957 < 200; num957++) {
						if (Main.npc[num957].aiStyle == NPC.aiStyle)
							Main.npc[num957].active = false;
					}
				}
			}

			float num188 = speed;
			float num189 = turnSpeed;
			Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num191 = player.position.X + (float)(player.width / 2);
			float num192 = player.position.Y + (float)(player.height / 2);
			int num42 = -1;
			int num43 = (int)(player.Center.X / 16f);
			int num44 = (int)(player.Center.Y / 16f);
			for (int num45 = num43 - 2; num45 <= num43 + 2; num45++) {
				for (int num46 = num44; num46 <= num44 + 15; num46++) {
					if (WorldGen.SolidTile2(num45, num46)) {
						num42 = num46;
						break;
					}
				}
				if (num42 > 0)
					break;
			}
			if (num42 > 0) {
				NPC.defense = 5;
				num42 *= 16;
				float num47 = (float)(num42 - 200); //was 800
				if (player.position.Y > num47) {
					num192 = num47;
					if (Math.Abs(NPC.Center.X - player.Center.X) < 125f) //was 500
					{
						if (NPC.velocity.X > 0f)
							num191 = player.Center.X + 150f; //was 600
						else
							num191 = player.Center.X - 150f; //was 600
					}
				}
			}
			else {
				NPC.defense = 0;
				num188 = expertMode ? 10.83f : 8.66f; //added 2.5
				num189 = expertMode ? 0.208f : 0.166f; //added 0.05
			}

			float num48 = num188 * 1.5f;
			float num49 = num188 * 0.8f;
			float num50 = NPC.velocity.Length();
			if (num50 > 0f) {
				if (num50 > num48) {
					NPC.velocity.Normalize();
					NPC.velocity *= num48;
				}
				else if (num50 < num49) {
					NPC.velocity.Normalize();
					NPC.velocity *= num49;
				}
			}

			if (num42 > 0) {
				for (int num51 = 0; num51 < 200; num51++) {
					if (Main.npc[num51].active && Main.npc[num51].type == NPC.type && num51 != NPC.whoAmI) {
						Vector2 vector3 = Main.npc[num51].Center - NPC.Center;
						if (vector3.Length() < 400f) {
							vector3.Normalize();
							vector3 *= 1000f;
							num191 -= vector3.X;
							num192 -= vector3.Y;
						}
					}
				}
			}
			else {
				for (int num52 = 0; num52 < 200; num52++) {
					if (Main.npc[num52].active && Main.npc[num52].type == NPC.type && num52 != NPC.whoAmI) {
						Vector2 vector4 = Main.npc[num52].Center - NPC.Center;
						if (vector4.Length() < 60f) {
							vector4.Normalize();
							vector4 *= 200f;
							num191 -= vector4.X;
							num192 -= vector4.Y;
						}
					}
				}
			}

			num191 = (float)((int)(num191 / 16f) * 16);
			num192 = (float)((int)(num192 / 16f) * 16);
			vector18.X = (float)((int)(vector18.X / 16f) * 16);
			vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
			num191 -= vector18.X;
			num192 -= vector18.Y;
			float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
			if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length) {
				try {
					vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num191 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
					num192 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
				}
				catch {
				}
				NPC.rotation = (float)System.Math.Atan2((double)num192, (double)num191) + 1.57f;
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				int num194 = NPC.width;
				num193 = (num193 - (float)num194) / num193;
				num191 *= num193;
				num192 *= num193;
				NPC.velocity = Vector2.Zero;
				NPC.position.X = NPC.position.X + num191;
				NPC.position.Y = NPC.position.Y + num192;
				if (directional) {
					if (num191 < 0f)
						NPC.spriteDirection = 1;
					if (num191 > 0f)
						NPC.spriteDirection = -1;
				}
			}
			else {
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				float num196 = System.Math.Abs(num191);
				float num197 = System.Math.Abs(num192);
				float num198 = num188 / num193;
				num191 *= num198;
				num192 *= num198;
				bool flag21 = false;
				if (!flag21) {
					if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f)) {
						if (NPC.velocity.X < num191)
							NPC.velocity.X = NPC.velocity.X + num189;
						else if (NPC.velocity.X > num191)
							NPC.velocity.X = NPC.velocity.X - num189;

						if (NPC.velocity.Y < num192)
							NPC.velocity.Y = NPC.velocity.Y + num189;
						else if (NPC.velocity.Y > num192)
							NPC.velocity.Y = NPC.velocity.Y - num189;

						if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f))) {
							if (NPC.velocity.Y > 0f)
								NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
							else
								NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
						}

						if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f))) {
							if (NPC.velocity.X > 0f)
								NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
							else
								NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
						}
					}
					else {
						if (num196 > num197) {
							if (NPC.velocity.X < num191)
								NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
							else if (NPC.velocity.X > num191)
								NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1

							if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5) {
								if (NPC.velocity.Y > 0f)
									NPC.velocity.Y = NPC.velocity.Y + num189;
								else
									NPC.velocity.Y = NPC.velocity.Y - num189;
							}
						}
						else {
							if (NPC.velocity.Y < num192)
								NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
							else if (NPC.velocity.Y > num192)
								NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;

							if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5) {
								if (NPC.velocity.X > 0f)
									NPC.velocity.X = NPC.velocity.X + num189;
								else
									NPC.velocity.X = NPC.velocity.X - num189;
							}
						}
					}
				}
			}
			NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<GravityModulator>(400);
			npcLoot.AddCommon<StarEnergy>(400);
			npcLoot.AddOneFromOptions(30, ModContent.ItemType<ProtectorateBody>(), ModContent.ItemType<ProtectorateLegs>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Stardancer1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Stardancer2").Type, 1f);
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 20;
				NPC.height = 20;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 5; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, .5f);
					Main.dust[num622].velocity *= 2f;
				}
				for (int num623 = 0; num623 < 10; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 4f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.DungeonSpirit, 0f, 0f, 100, default, .5f);
					Main.dust[num624].velocity *= 1f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Color col = NPC.IsABestiaryIconDummy ? Color.White : drawColor;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, col, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			drawOrigin.Y += 30f;
			drawOrigin.Y += 8f;
			--drawOrigin.X;
			Vector2 position1 = NPC.Bottom - Main.screenPosition;
			Texture2D texture2D2 = TextureAssets.GlowMask[239].Value;
			float num11 = (float)((double)Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
			float num12 = num11;
			if ((double)num12 > 0.5)
				num12 = 1f - num11;
			if ((double)num12 < 0.0)
				num12 = 0.0f;
			float num13 = (float)(((double)num11 + 0.5) % 1.0);
			float num14 = num13;
			if ((double)num14 > 0.5)
				num14 = 1f - num13;
			if ((double)num14 < 0.0)
				num14 = 0.0f;
			Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
			drawOrigin = r2.Size() / 2f;
			Vector2 position3 = position1 + new Vector2(0.0f, -20f);
			Color color3 = new Color(84, 207, 255) * 1.6f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3, NPC.rotation, drawOrigin, NPC.scale * 0.35f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num12, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num14, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Starfarer/CogTrapperHead_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, screenPos);
		}
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class TailProbe : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 120f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfarer");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 56;
			NPC.height = 46;
			NPC.damage = 0;
			NPC.defense = 12;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.value = 160f;
			NPC.knockBackResist = .16f;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			int r1 = (int)color1.R;
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
			Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
			drawOrigin = r2.Size() / 2f;
			Vector2 position3 = position1 + new Vector2(0.0f, -20f);
			Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(255, 138, 36) * 1.6f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3, NPC.rotation, drawOrigin, NPC.scale * .75f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num12, NPC.rotation, drawOrigin, NPC.scale * .75f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num14, NPC.rotation, drawOrigin, NPC.scale * .75f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			Texture2D texture2D3 = TextureAssets.Extra[89].Value;
			Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
			drawOrigin = r3.Size() / 2f;
			Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
			float num17 = 1f + num13 * 0.75f;

			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/SteamRaider/TailProbe_Glow").Value, screenPos);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (NPC.width / 2.0f);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2.0f);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2.0f);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2.0f);
				for (int num621 = 0; num621 < 10; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 1f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		int shoottimer;
		public override void AI()
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<SteamRaiderHead>())) {
				NPC.active = false;
				NPC.position.X = NPC.position.X + (NPC.width / 2.0f);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2.0f);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2.0f);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2.0f);
				for (int num621 = 0; num621 < 10; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, .8f);
					Main.dust[num622].velocity *= 1f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
					}
				}
				return;
			}
			Player player = Main.player[NPC.target];
			/*for (int i = 0; i < 255; ++i)
					{
						if (Main.player[i].active && !Main.player[i].dead)
						{
							if ((Main.player[i].Center - npc.Center).Length() <= 120)
							{
								//Main.player[i].Hurt(1, 0, false, false, " was evaporated...", false, 1); commed out because this needs work
								Main.player[i].AddBuff(BuffID.Slow, 90);
							}
						}
					}*/
			int parent = NPC.FindFirstNPC(ModContent.NPCType<SteamRaiderHead>());
			{
				if (Main.npc[parent].life <= Main.npc[parent].lifeMax * .2f) {
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
			}
			float num5 = base.NPC.position.X + (float)(base.NPC.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = base.NPC.position.Y + (float)base.NPC.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
			if (num7 < 0f) {
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283) {
				num7 -= 6.283f;
			}
			float num8 = 0.1f;
			if (base.NPC.rotation < num7) {
				if ((double)(num7 - base.NPC.rotation) > 3.1415) {
					base.NPC.rotation -= num8;
				}
				else {
					base.NPC.rotation += num8;
				}
			}
			else if (base.NPC.rotation > num7) {
				if ((double)(base.NPC.rotation - num7) > 3.1415) {
					base.NPC.rotation += num8;
				}
				else {
					base.NPC.rotation -= num8;
				}
			}
			if (base.NPC.rotation > num7 - num8 && base.NPC.rotation < num7 + num8) {
				base.NPC.rotation = num7;
			}
			if (base.NPC.rotation < 0f) {
				base.NPC.rotation += 6.283f;
			}
			else if ((double)base.NPC.rotation > 6.283) {
				base.NPC.rotation -= 6.283f;
			}
			if (base.NPC.rotation > num7 - num8 && base.NPC.rotation < num7 + num8) {
				base.NPC.rotation = num7;
			}
			base.NPC.spriteDirection = base.NPC.direction;
			if (Main.npc[parent].ai[2] == 0)
			{
				shoottimer++;
				if (shoottimer >= 120 && shoottimer <= 180)
				{
					int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].scale *= .8f;
					Main.dust[dust].noGravity = true;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-80, 81), (float)Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = NPC.Center - vector2_3;

					if (shoottimer >= 180 && shoottimer % 10 == 0)
					{
						SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
						Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
						direction.Normalize();
						direction.X *= 6f;
						direction.Y *= 6f;

						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<GlitchLaser>(), NPCUtils.ToActualDamage(32, 1.5f), 1, Main.myPlayer, 0, 0);

						if (!Main.expertMode || shoottimer >= 200)
							shoottimer = 0;
					}

				}
			}
			else
				shoottimer = 0;

			NPC.TargetClosest(true);
			if (NPC.direction == -1) {
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction *= 12f;
			}
			if (NPC.direction == 1) {
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction *= -12f;
			}
			if (NPC.Center.X >= player.Center.X && moveSpeed >= -80) // flies to players x position
			{
				moveSpeed--;
			}

			if (NPC.Center.X <= player.Center.X && moveSpeed <= 80) {
				moveSpeed++;
			}

			NPC.velocity.X = moveSpeed * 0.12f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -27) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 120f;
			}

			if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 27) {
				moveSpeedY++;
			}

			NPC.velocity.Y = moveSpeedY * 0.14f;
			if (Main.rand.Next(220) == 33) {
				HomeY = -35f;
			}
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .25f, .57f, .85f);
			NPC.spriteDirection = NPC.direction;
		}
	}
}

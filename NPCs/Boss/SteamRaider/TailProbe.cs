using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 46;
			npc.damage = 0;
			npc.defense = 12;
			npc.noTileCollide = true;
			npc.dontTakeDamage = true;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit4;
			npc.value = 160f;
			npc.knockBackResist = .16f;
			npc.noGravity = true;
			npc.dontCountMe = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
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
			Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			int r1 = (int)color1.R;
			drawOrigin.Y += 30f;
			drawOrigin.Y += 8f;
			--drawOrigin.X;
			Vector2 position1 = npc.Bottom - Main.screenPosition;
			Texture2D texture2D2 = Main.glowMaskTexture[239];
			float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
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
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * .75f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * .75f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * .75f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			Texture2D texture2D3 = Main.extraTexture[89];
			Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
			drawOrigin = r3.Size() / 2f;
			Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
			float num17 = 1f + num13 * 0.75f;

			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/TailProbe_Glow"));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				npc.position.X = npc.position.X + (npc.width / 2.0f);
				npc.position.Y = npc.position.Y + (npc.height / 2.0f);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2.0f);
				npc.position.Y = npc.position.Y - (npc.height / 2.0f);
				for (int num621 = 0; num621 < 10; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 2f);
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
				npc.active = false;
				npc.position.X = npc.position.X + (npc.width / 2.0f);
				npc.position.Y = npc.position.Y + (npc.height / 2.0f);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2.0f);
				npc.position.Y = npc.position.Y - (npc.height / 2.0f);
				for (int num621 = 0; num621 < 10; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), .8f);
					Main.dust[num622].velocity *= 1f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
					}
				}
			}
			Player player = Main.player[npc.target];
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
				if (Main.npc[parent].life <= Main.npc[parent].lifeMax * .3f) {
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.active = false;
				}
			}
			float num5 = base.npc.position.X + (float)(base.npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = base.npc.position.Y + (float)base.npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
			if (num7 < 0f) {
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283) {
				num7 -= 6.283f;
			}
			float num8 = 0.1f;
			if (base.npc.rotation < num7) {
				if ((double)(num7 - base.npc.rotation) > 3.1415) {
					base.npc.rotation -= num8;
				}
				else {
					base.npc.rotation += num8;
				}
			}
			else if (base.npc.rotation > num7) {
				if ((double)(base.npc.rotation - num7) > 3.1415) {
					base.npc.rotation += num8;
				}
				else {
					base.npc.rotation -= num8;
				}
			}
			if (base.npc.rotation > num7 - num8 && base.npc.rotation < num7 + num8) {
				base.npc.rotation = num7;
			}
			if (base.npc.rotation < 0f) {
				base.npc.rotation += 6.283f;
			}
			else if ((double)base.npc.rotation > 6.283) {
				base.npc.rotation -= 6.283f;
			}
			if (base.npc.rotation > num7 - num8 && base.npc.rotation < num7 + num8) {
				base.npc.rotation = num7;
			}
			base.npc.spriteDirection = base.npc.direction;
			shoottimer++;
			if (shoottimer >= 120 && shoottimer <= 180) {
				{
					int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].scale *= .8f;
					Main.dust[dust].noGravity = true;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-80, 81), (float)Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = npc.Center - vector2_3;
				}
			}
			{
				if (shoottimer >= 180) {
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 12);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 6f;
					direction.Y *= 6f;

					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<GlitchLaser>(), 17, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
					}
					shoottimer = 0;
				}
			}

			npc.TargetClosest(true);
			if (npc.direction == -1) {
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction *= 12f;
			}
			if (npc.direction == 1) {
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction *= -12f;
			}
			if (npc.Center.X >= player.Center.X && moveSpeed >= -80) // flies to players x position
			{
				moveSpeed--;
			}

			if (npc.Center.X <= player.Center.X && moveSpeed <= 80) {
				moveSpeed++;
			}

			npc.velocity.X = moveSpeed * 0.09f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -27) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 120f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 27) {
				moveSpeedY++;
			}

			npc.velocity.Y = moveSpeedY * 0.1f;
			if (Main.rand.Next(220) == 33) {
				HomeY = -35f;
			}
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .25f, .57f, .85f);
			npc.spriteDirection = npc.direction;
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class LeafProjReachChest : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leaf Arrow");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 16;
			projectile.aiStyle = 43;
			aiType = 227;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.minionSlots = 0;
			projectile.penetrate = 3;
			projectile.timeLeft = 180;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if(projectile.frameCounter > 8) {
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if(projectile.frame > 4) {
				projectile.frame = 0;
			}
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.025f) / 255f, ((255 - projectile.alpha) * 0.25f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);
			projectile.velocity.Y += projectile.ai[0];
			if(Main.rand.Next(8) == 0) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 3, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[d].scale *= .5f;
			}
			if(Main.myPlayer == projectile.owner && projectile.ai[0] <= 0f) {
				if(Main.player[projectile.owner].channel && channelable) {
					float num2353 = 12f;
					Vector2 vector329 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
					float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
					if(Main.player[projectile.owner].gravDir == -1f) {
						num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
					}
					float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					if(projectile.ai[0] < 0f) {
						projectile.ai[0] += 1f;
					}
					if(projectile.type == 491 && num2350 < 100f) {
						if(projectile.velocity.Length() < num2353) {
							projectile.velocity *= 1.1f;
							if(projectile.velocity.Length() > num2353) {
								projectile.velocity.Normalize();
								projectile.velocity *= num2353;
							}
						}
						if(projectile.ai[0] == 0f) {
							projectile.ai[0] = -10f;
						}
					} else if(num2350 > num2353) {
						num2350 = num2353 / num2350;
						num2352 *= num2350;
						num2351 *= num2350;
						int num2345 = (int)(num2352 * 1000f);
						int num2344 = (int)(projectile.velocity.X * 1000f);
						int num2343 = (int)(num2351 * 1000f);
						int num2342 = (int)(projectile.velocity.Y * 1000f);
						if(num2345 != num2344 || num2343 != num2342) {
							projectile.netUpdate = true;
						}
						if(projectile.type == 491) {
							Vector2 value167 = new Vector2(num2352, num2351);
							projectile.velocity = (projectile.velocity * 4f + value167) / 5f;
						} else {
							projectile.velocity.X = num2352;
							projectile.velocity.Y = num2351;
						}
					} else {
						int num2341 = (int)(num2352 * 1000f);
						int num2340 = (int)(projectile.velocity.X * 1000f);
						int num2339 = (int)(num2351 * 1000f);
						int num2338 = (int)(projectile.velocity.Y * 1000f);
						if(num2341 != num2340 || num2339 != num2338) {
							projectile.netUpdate = true;
						}
						projectile.velocity.X = num2352;
						projectile.velocity.Y = num2351;
					}
				} else if(projectile.ai[0] <= 0f) {
					projectile.netUpdate = true;
					if(projectile.type != 491) {
						float num2337 = 16f;
						Vector2 vector328 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
						float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
						float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
						if(Main.player[projectile.owner].gravDir == -1f) {
							num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
						}
						float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						if(num2334 == 0f || projectile.ai[0] < 0f) {
							vector328 = new Vector2(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2), Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2));
							num2336 = projectile.position.X + (float)projectile.width * 0.5f - vector328.X;
							num2335 = projectile.position.Y + (float)projectile.height * 0.5f - vector328.Y;
							num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						}
						num2334 = num2337 / num2334;
						num2336 *= num2334;
						num2335 *= num2334;
						projectile.velocity.X = num2336;
						projectile.velocity.Y = num2335;
						if(projectile.velocity.X == 0f && projectile.velocity.Y == 0f) {
							projectile.Kill();
						}
					}
					channelable = false;
					projectile.ai[0] = 1f;
				}
			}
		}
		bool channelable = true;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, 100);
		}
		public override void Kill(int timeLeft)
		{
			for(int k = 0; k < 3; k++) {
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 3, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
			}
		}
	}
}
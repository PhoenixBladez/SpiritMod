using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AquaFlareProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquaflare");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.aiStyle = -1;
			projectile.penetrate = 3;
			projectile.scale = 1.25f;
			projectile.timeLeft = 240;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(0, 98, 255) * .95f), new RoundCap(), new SleepingStarTrailPosition(), 18f, 450f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(255, 68, 0)), new RoundCap(), new SleepingStarTrailPosition(), 18f, 330f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(107, 211, 255) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 12f, 80f, new DefaultShader());
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 210, 133, 100);
		bool noChannel;
		public override void AI()
		{
			if (Main.myPlayer == projectile.owner) 
			{
				projectile.netUpdate = true;
			}
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.25f) / 255f, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);
			if (Main.rand.Next(8) == 0) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[d].scale *= 1.85f;
				Main.dust[d].noGravity = true;
			}
			if (Main.rand.Next(8) == 0) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.DungeonWater, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[d].scale *= 1.85f;
				Main.dust[d].noGravity = true;
			}
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			Player player = Main.player[projectile.owner];
			if (Main.player[projectile.owner].channel && !noChannel) {
				float num2353 = 45f;
				Vector2 vector329 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
				float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
				if (Main.player[projectile.owner].gravDir == -1f) {
					num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
				}
				float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
				num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
				if (projectile.ai[0] < 0f) {
					projectile.ai[0] += 1f;
				}
				if (projectile.type == 491 && num2350 < 100f) {
					if (projectile.velocity.Length() < num2353) {
						projectile.velocity *= 1.1f;
						if (projectile.velocity.Length() > num2353) {
							projectile.velocity.Normalize();
							projectile.velocity *= num2353;
						}
					}
					if (projectile.ai[0] == 0f) {
						projectile.ai[0] = -10f;
					}
				}
				else if (num2350 > num2353) {
					num2350 = num2353 / num2350;
					num2352 *= num2350;
					num2351 *= num2350;
					int num2345 = (int)(num2352 * 1000f);
					int num2344 = (int)(projectile.velocity.X * 1000f);
					int num2343 = (int)(num2351 * 1000f);
					int num2342 = (int)(projectile.velocity.Y * 1000f);
					if (num2345 != num2344 || num2343 != num2342) {
						projectile.netUpdate = true;
					}
					if (projectile.type == 491) {
						Vector2 value167 = new Vector2(num2352, num2351);
						projectile.velocity = (projectile.velocity * 4f + value167) / 5f;
					}
					else {
						projectile.velocity.X = num2352;
						projectile.velocity.Y = num2351;
					}
				}
				else {
					int num2341 = (int)(num2352 * 1000f);
					int num2340 = (int)(projectile.velocity.X * 1000f);
					int num2339 = (int)(num2351 * 1000f);
					int num2338 = (int)(projectile.velocity.Y * 1000f);
					if (num2341 != num2340 || num2339 != num2338) {
						projectile.netUpdate = true;
					}
					projectile.velocity.X = num2352;
					projectile.velocity.Y = num2351;
				}
			}
			else if (projectile.ai[0] <= 0f) {

				projectile.netUpdate = true;
				if (projectile.type != 491) {
					float num2337 = 33f;
					Vector2 vector328 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
					float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
					if (Main.player[projectile.owner].gravDir == -1f) {
						num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
					}
					float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
					if (num2334 == 0f || projectile.ai[0] < 0f) {
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
					if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f) {
						projectile.Kill();
					}
				}
				projectile.ai[0] = 1f;
			}
			else {
				noChannel = true;
				projectile.netUpdate = true;
				bool flag25 = false;
				int jim = 1;
				for (int index1 = 0; index1 < 200; index1++) {
					if (Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
						float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
						float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
						float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
						if (num25 < 500f) {
							flag25 = true;
							jim = index1;
						}

					}
				}
				if (flag25) {
					float num1 = 21.5f;
					Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num2 = Main.npc[jim].Center.X - vector2.X;
					float num3 = Main.npc[jim].Center.Y - vector2.Y;
					float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
					float num5 = num1 / num4;
					float num6 = num2 * num5;
					float num7 = num3 * num5;
					int num8 = 10;
					{
						projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
						projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
					}
				}
			}
		}

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
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.OnFire, 180);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			for (int k = 0; k < 16; k++) {
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, 0f, 0f);
			}
			for (int i = 0; i < 16; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, -2f, 117, new Color(0, 255, 142), 1.7f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}
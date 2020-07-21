using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class SleepingStar1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sleeping Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.ranged = true;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200, true);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			{
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 223, 0f, -2f, 0, default(Color), .6f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != projectile.Center) {
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
					}
				}

			}

		}
		bool looping = false;
		int loopCounter = 0;
		//bool dustSpawn = true;
		int loopSize = 9;
		public override void AI()
		{
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.396f, 0.170588235f, 0.564705882f);
			//dustSpawn = false;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (!looping) //change direction slightly
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			if (Main.rand.Next(100) == 1 && !looping) {
				looping = true;
				loopSize = Main.rand.Next(8, 14);
				loopCounter = 0;
			}
			if (looping) {
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Math.PI / loopSize);
				loopCounter++;
				if (loopCounter >= loopSize * 2) {
					looping = false;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(160, 160, 160, 100);
		}
	}
}

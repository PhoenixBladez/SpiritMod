
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class FaerieStar : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Faerie Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.height = 26;
			projectile.width = 26;
			projectile.penetrate = 5;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

               
			}
			return true;
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            if (projectile.timeLeft < 196)
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = Main.projectileTexture[projectile.type];
                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * .5f, projectile.rotation, tex.Size() / 2, scale, default, default);
              
                }
            }
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int i = 0; i < 16; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default(Color), .8f);
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
		public override void AI()
		{
            if (Main.rand.NextBool(10))
            {
                 DustHelper.DrawStar(new Vector2(projectile.Center.X + Main.rand.Next(-20, 20), projectile.Center.Y + Main.rand.Next(-20, 20)), 187, pointAmount: 5, mainSize: 2.2425f, dustDensity: 1.5f, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
            }
			projectile.rotation += .15f;
			Lighting.AddLight(projectile.position, .055f, .054f, .223f);
		}
	}
}

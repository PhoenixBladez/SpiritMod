using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Bullet
{
	public class MoonshotBulletLarge : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 2;
            projectile.ranged = true;
            projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		bool initialized = false;
        float alphaCounter;
		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
            Vector2 center = projectile.Center;
            float num8 = (float)player.miscCounter / 40f;
            float num7 = 1.0471975512f * 2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num6 = Dust.NewDust(center, 0, 0, 226, 0f, 0f, 100, default(Color), .75f);
                    Main.dust[num6].noGravity = true;
                    Main.dust[num6].velocity = Vector2.Zero;
                    Main.dust[num6].noLight = true;
                    Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 9f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .3f), SpriteEffects.None, 0f);
        }
        public override void Kill(int timeLeft)
        {
            ProjectileExtras.Explode(projectile.whoAmI, 60, 60, delegate
            {
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3));
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].scale *= .25f;
                            if (Main.dust[num].position != projectile.Center)
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                    DustHelper.DrawDustImage(projectile.Center, 226, 0.25f, "SpiritMod/Effects/DustImages/MoonSigil2", 1f);
                }
            }, true);

        }
    }
}
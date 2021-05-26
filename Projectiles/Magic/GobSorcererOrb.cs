using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GobSorcererOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Stalactite");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
            projectile.timeLeft = 150;
			projectile.magic = true;
			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
            Player player = Main.player[projectile.owner];
            projectile.velocity *= .98f;
            projectile.rotation += .1f;
            Vector2 center = projectile.Center;
            float num8 = (float)player.miscCounter / 40f;
            float num7 = 1.0471975512f*2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num6 = Dust.NewDust(center, 0, 0, 173, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num6].noGravity = true;
                    Main.dust[num6].velocity = Vector2.Zero;
                    Main.dust[num6].noLight = true;
                    Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 8f;
                }
            }
            return false;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(211, 181, 255, 100);
        }
        public override void Kill(int timeLeft)
		{
            Main.PlaySound(SoundID.DD2_WitherBeastHurt, projectile.Center);
            for (int I = 0; I < 3; I++)
            {
                Vector2 pos = new Vector2(projectile.Center.X + Main.rand.Next(-15, 15), projectile.Center.Y + Main.rand.Next(-15, 15));
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(pos, 0, 0, pos + spawnPlace, 0, 0))
                {
                    pos += spawnPlace;
                }

                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - pos) * 8f;
                int p = Projectile.NewProjectile(pos.X, pos.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Projectiles.Magic.ShadowflameOrbBolt>(), projectile.damage, projectile.knockBack, 0, 0.0f, 0.0f);
                for (float num2 = 0.0f; (double)num2 < 10; ++num2)
                {
                    int dustIndex = Dust.NewDust(pos, 2, 2, 173, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 2.1f;
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
	}
}

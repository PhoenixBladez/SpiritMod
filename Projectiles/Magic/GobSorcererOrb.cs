using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GobSorcererOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Stalactite");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
            Projectile.timeLeft = 150;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;

			Projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= .98f;
            Projectile.rotation += .1f;
            Vector2 center = Projectile.Center;
            float num8 = (float)player.miscCounter / 40f;
            float num7 = 1.0471975512f*2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num6 = Dust.NewDust(center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1f);
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
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, Projectile.Center);
            for (int I = 0; I < 3; I++)
            {
                Vector2 pos = new Vector2(Projectile.Center.X + Main.rand.Next(-15, 15), Projectile.Center.Y + Main.rand.Next(-15, 15));
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(pos, 0, 0, pos + spawnPlace, 0, 0))
                {
                    pos += spawnPlace;
                }

                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - pos) * 8f;
                int p = Projectile.NewProjectile(pos.X, pos.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Projectiles.Magic.ShadowflameOrbBolt>(), Projectile.damage, Projectile.knockBack, 0, 0.0f, 0.0f);
                for (float num2 = 0.0f; (double)num2 < 10; ++num2)
                {
                    int dustIndex = Dust.NewDust(pos, 2, 2, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 2.1f;
                }
            }
        }
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

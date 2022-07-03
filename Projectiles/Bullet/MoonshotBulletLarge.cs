using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Bullet
{
	public class MoonshotBulletLarge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 300;
			Projectile.damage = 13;
			//projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

		}

		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 center = Projectile.Center;
			float num8 = (float)player.miscCounter / 40f;
			float num7 = 1.0471975512f * 2;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					int num6 = Dust.NewDust(center, 0, 0, DustID.Electric, 0f, 0f, 100, default, .75f);
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
		public override void PostDraw(Color lightColor)
		{
			float sineAdd = (float)Math.Sin(0) + 3;
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Effects/Masks/Extra_49", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (Projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .3f), SpriteEffects.None, 0f);
		}
		public override void Kill(int timeLeft)
		{
			ProjectileExtras.Explode(Projectile.whoAmI, 60, 60, delegate
			{
				SoundEngine.PlaySound(SoundID.NPCHit3);
				for (int i = 0; i < 10; i++)
				{
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != Projectile.Center)
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
				DustHelper.DrawDustImage(Projectile.Center, 226, 0.25f, "SpiritMod/Effects/DustImages/MoonSigil2", 1f);
			}, true);
		}
	}
}
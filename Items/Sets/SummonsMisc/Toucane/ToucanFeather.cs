using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.Toucane
{
	public class ToucanFeather : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toucan Feather");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		private const int maxtimeleft = 360;
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10, 10);
			Projectile.scale = Main.rand.NextFloat(0.5f, 0.6f);
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = maxtimeleft;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.alpha = Math.Max(Projectile.alpha - 20, 0);
			if (Projectile.timeLeft > (maxtimeleft - 10))
				Projectile.scale *= 1.08f;

			if (Projectile.velocity.Length() < 12)
				Projectile.velocity *= 1.03f;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if(!Projectile.wet)
				Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() / 2);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 90, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.5f, 0.7f), 100, default, Main.rand.NextFloat(0.7f, 1f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}

			for (int j = 0; j < 10; j++)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 90, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.1f, 0.3f), 100, default, Main.rand.NextFloat(0.2f, 0.4f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(spriteBatch);
			Projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (!Projectile.wet)
			{
				Texture2D bloom = Mod.GetTexture("Effects/Desert_Shadow");

				Color color = Color.Lerp(new Color(255, 0, 89), new Color(255, 47, 0), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f);
				color.A = (byte)(color.A * 2);
				spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation, bloom.Size() / 2, Projectile.scale * 1.5f, SpriteEffects.None, 0);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (Projectile.wet)
				return;
			Color color = Color.Lerp(new Color(255, 0, 89), new Color(255, 47, 0), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f) * 0.7f * Projectile.Opacity;
			Projectile.QuickDrawGlowTrail(spriteBatch, 0.9f, color);
			Projectile.QuickDrawGlow(spriteBatch, color);
		}
	}
}
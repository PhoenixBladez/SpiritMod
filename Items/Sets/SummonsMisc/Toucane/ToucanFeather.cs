using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.Toucane
{
	public class ToucanFeather : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toucan Feather");
			ProjectileID.Sets.MinionShot[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private const int maxtimeleft = 360;
		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.scale = Main.rand.NextFloat(0.5f, 0.6f);
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = maxtimeleft;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 20, 0);
			if (projectile.timeLeft > (maxtimeleft - 10))
				projectile.scale *= 1.08f;

			if (projectile.velocity.Length() < 12)
				projectile.velocity *= 1.03f;

			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if(!projectile.wet)
				Lighting.AddLight(projectile.Center, Color.Red.ToVector3() / 2);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.Dig, projectile.Center);

			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, 90, projectile.velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.5f, 0.7f), 100, default, Main.rand.NextFloat(0.7f, 1f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}

			for (int j = 0; j < 10; j++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, 90, projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.1f, 0.3f), 100, default, Main.rand.NextFloat(0.2f, 0.4f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch);
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!projectile.wet)
			{
				Texture2D bloom = mod.GetTexture("Effects/Desert_Shadow");

				Color color = Color.Lerp(new Color(255, 0, 89), new Color(255, 47, 0), (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f);
				color.A = (byte)(color.A * 2);
				spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, color * 0.5f, projectile.rotation, bloom.Size() / 2, projectile.scale * 1.5f, SpriteEffects.None, 0);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (projectile.wet)
				return;
			Color color = Color.Lerp(new Color(255, 0, 89), new Color(255, 47, 0), (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f) * 0.7f * projectile.Opacity;
			projectile.QuickDrawGlowTrail(spriteBatch, 0.9f, color);
			projectile.QuickDrawGlow(spriteBatch, color);
		}
	}
}
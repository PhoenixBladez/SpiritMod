using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunBullet : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granitech Bullet");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.width = 46;
			projectile.height = 8;
			projectile.alpha = 0;
			projectile.timeLeft = 900;
			projectile.aiStyle = -1;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.Pi;

			if (projectile.ai[0] % 1 == 0 && projectile.ai[0] >= 1 && projectile.ai[0] < 3)
			{
				//var d = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<GranitechGunDust>(), Vector2.Zero, 0, default, Main.rand.NextFloat(1f, 1.5f));
				//GranitechGunDust.SetFrame(d, 3);
				//d.rotation = projectile.velocity.ToRotation() - MathHelper.Pi;
				//d.fadeIn = Main.rand.NextFloat(1f, 1.6f) * (0.75f + projectile.ai[0]);
				//d.velocity = projectile.velocity * d.fadeIn * 0.08f;
				//d.scale = 0f;

				float scale = 0.75f + (2 - (projectile.ai[0] * 0.75f));
				ParticleHandler.SpawnParticle(new GranitechGunParticle(projectile.Center, projectile.velocity * 0.2f * (2.5f - scale), new Color(255, 46, 122) * 0.5f, scale, 60));
			}
			projectile.ai[0]++;
		}

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(projectile, new GranitechBulletTrail(new Color(255, 46, 122, 0)), new RoundCap(), new DefaultTrailPosition(), 4, 220);

		internal class GranitechBulletTrail : ITrailColor
		{
			private Color _colour;

			public GranitechBulletTrail(Color colour)
			{
				_colour = colour;
			}

			public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points) => _colour;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(29, 53).WithPitchVariance(0.3f), projectile.Center);
			for (int i = 0; i < 20; i++)
			{
				Vector2 vel = projectile.velocity.RotatedByRandom(MathHelper.Pi / 8f) * Main.rand.NextFloat(0.2f, 0.6f);
				var d = Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(4, 4), ModContent.DustType<GranitechGunDust>(), vel, 0, default, Main.rand.NextFloat(1f, 1.5f));
				GranitechGunDust.RandomizeFrame(d);
			}
		}
	}
}

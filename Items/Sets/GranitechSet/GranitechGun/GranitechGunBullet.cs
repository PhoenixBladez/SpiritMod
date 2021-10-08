using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpiritMod.Particles;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunBullet : ModProjectile, IDrawAdditive //, ITrailProjectile
	{
		public bool spawnRings = false;

		private Color _glowColor = new Color(255, 46, 122);

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
			projectile.extraUpdates = 1;

			//if (Main.rand.NextBool())
				_glowColor = new Color(239, 241, 80);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.Pi;

			if (spawnRings && projectile.ai[0] % 1 == 0 && projectile.ai[0] >= 1 && projectile.ai[0] < 3)
			{
				float scale = 0.75f + (2 - (projectile.ai[0] * 0.75f));
				ParticleHandler.SpawnParticle(new GranitechGunParticle(projectile.Center, projectile.velocity * 0.2f * (2f - (scale * 0.5f)), scale, 50));
			}
			projectile.ai[0]++;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(29, 53).WithPitchVariance(0.3f), projectile.Center);
			for (int i = 0; i < 3; i++)
			{
				Vector2 vel = projectile.velocity.RotatedByRandom(MathHelper.Pi / 8f) * Main.rand.NextFloat(0.2f, 0.6f);
				var d = Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(4, 4), ModContent.DustType<GranitechGunDust>(), vel, 0, default, Main.rand.NextFloat(1f, 1.5f));
				GranitechGunDust.RandomizeFrame(d);
			}
			spawnRings = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPosition = projectile.Center - Main.screenPosition;

			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPosition, null, Color.White, projectile.rotation, new Vector2(10, 5), 1f, SpriteEffects.None, 0f);

			Vector2 normVel = Vector2.Normalize(projectile.velocity);
			Texture2D glow = mod.GetTexture("Items/Sets/GranitechSet/GranitechGun/GranitechGunBullet_Glow");

			Color[] colors = new Color[2] { new Color(222, 111, 127), new Color(178, 105, 140) };
			for (int i = 0; i < 2; ++i)
			{
				Vector2 off = normVel * 16 * (i + 1);
				spriteBatch.Draw(glow, drawPosition - off, null, colors[i] * (1 - (i * 0.05f)), projectile.rotation, new Vector2(10, 5), 1f, SpriteEffects.None, 0f);
			}
			return false;
		}

		public void AdditiveCall(SpriteBatch sb)
		{
			Texture2D bloomTex = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");

			sb.Draw(bloomTex, projectile.Center - Main.screenPosition, null, Color.White * 0.5f, 0, bloomTex.Size() / 2f, 0.2f, SpriteEffects.None, 0);
		}

		//public void DoTrailCreation(TrailManager tManager) { }// tManager.CreateTrail(projectile, new GranitechBulletTrail(new Color(255, 46, 122, 0)), new RoundCap(), new DefaultTrailPosition(), 4, 220);

		//internal class GranitechBulletTrail : ITrailColor
		//{
		//	private Color _colour;

		//	public GranitechBulletTrail(Color colour)
		//	{
		//		_colour = colour;
		//	}

		//	public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points) => _colour;
		//}
	}
}

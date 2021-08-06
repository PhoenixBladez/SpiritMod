using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.BaseProj;
using Terraria;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Items.Sets.LaunchersMisc.IronBomber
{
	public class IronBomberProj : BaseRocketProj, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Grenade");
			Main.projFrames[projectile.type] = 4;
		}

		private const int MaxTimeLeft = 240;
		private float PulseTime => (MaxTimeLeft * 0.66f);
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = MaxTimeLeft;
			projectile.ranged = true;
			projectile.Size = Vector2.One * 10;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StandardColorTrail(Color.LightCyan * 0.2f), new RoundCap(), new DefaultTrailPosition(), 25, 70);
			tM.CreateTrail(projectile, new StandardColorTrail(Color.Cyan), new NoCap(), new DefaultTrailPosition(), 10, 70);
		}

		public override void AI()
		{
			projectile.rotation += projectile.velocity.X * 0.1f;
			projectile.velocity.Y += 0.3f;
			projectile.velocity.X *= 0.97f;

			projectile.frameCounter++;
			int framespersecond = 10;
			if(projectile.frameCounter >= (60/ framespersecond))
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}

			if (projectile.timeLeft < PulseTime)
				projectile.scale = 1f + (float)Math.Sin(Math.Pow(projectile.timeLeft / PulseTime, 0.5) * MathHelper.Pi * 6)/4;

			if (!Main.dedServ && projectile.timeLeft % 4 == 3)
				ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, Color.Cyan * 0.6f, 20, 15));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Bounce(oldVelocity, 0.4f);
			return false;
		}

		public override int ExplosionRange => 150;

		public override void ExplodeEffect()
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BassBoom").WithVolume(0.7f).WithPitchVariance(0.2f), projectile.Center);

			ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, Vector2.Zero, Color.LightCyan * 0.6f, 1f, 10, 6));
			for (int i = 1; i <= 3; i++)
				ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, Color.Cyan * 0.8f, (ExplosionRange / 3) * i, 10));

			for(int i = 0; i < 10; i++)
				ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center + Main.rand.NextVector2Circular(ExplosionRange / 3, ExplosionRange / 3), Color.Cyan * 0.8f, (ExplosionRange / 5), 15));

			for (int i = 0; i < 15; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(ExplosionRange / 20), Color.Cyan, Main.rand.NextFloat(0.04f, 0.08f), 15));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			projectile.QuickDrawGlow(spriteBatch);
			if (projectile.timeLeft < PulseTime)
			{
				float Opacity = (float)Math.Pow((PulseTime - projectile.timeLeft) / PulseTime, 2);
				spriteBatch.Draw(ModContent.GetTexture(Texture + "_mask"), projectile.Center - Main.screenPosition, projectile.DrawFrame(), Color.Cyan * Opacity, projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
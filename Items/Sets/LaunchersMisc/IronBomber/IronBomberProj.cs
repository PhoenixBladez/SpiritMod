using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.BaseProj;
using Terraria;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Mechanics.Trails;

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
			Color color = Color.Cyan;
			color.A = 50;
			tM.CreateTrail(projectile, new StandardColorTrail(color), new NoCap(), new DefaultTrailPosition(), 10, 70);
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

			if (!Main.dedServ)
			{
				if(projectile.velocity.Length() > 6 && projectile.timeLeft % 4 == 3)
					ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, Color.Cyan, 18, 18) { Angle = projectile.velocity.ToRotation(), ZRotation = 0.5f });

				if(projectile.timeLeft % 6 == 5)
					ParticleHandler.SpawnParticle(new PulseCircle(projectile, Color.Cyan * 0.5f, 20, 20));
			}
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

			for (int i = 1; i <= 3; i++)
				ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, Color.Cyan * 0.5f, (ExplosionRange / 3) * i, 15, PulseCircle.MovementType.OutwardsQuadratic));

			for(int i = 0; i < 10; i++)
			{
				Vector2 offset = Main.rand.NextVector2Circular(ExplosionRange / 12, ExplosionRange / 12);
				ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center + offset, Color.Cyan * 0.7f, (ExplosionRange / 6), Main.rand.Next(14, 22))
				{ Velocity = Vector2.Normalize(offset) * Main.rand.NextFloat(1f, 1.5f), Angle = offset.ToRotation() });
			}
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
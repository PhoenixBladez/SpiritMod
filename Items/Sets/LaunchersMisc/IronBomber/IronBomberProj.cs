using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.BaseProj;
using Terraria;
using SpiritMod.Particles;
using Terraria.Audio;
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
			Main.projFrames[Projectile.type] = 4;
		}

		private const int MaxTimeLeft = 240;
		private float PulseTime => (MaxTimeLeft * 0.66f);
		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.Size = Vector2.One * 10;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new StandardColorTrail(Color.LightCyan * 0.2f), new RoundCap(), new DefaultTrailPosition(), 25, 70);
			Color color = Color.Cyan;
			color.A = 50;
			tM.CreateTrail(Projectile, new StandardColorTrail(color), new NoCap(), new DefaultTrailPosition(), 10, 70);
		}

		public override void AI()
		{
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			Projectile.velocity.Y += 0.3f;
			Projectile.velocity.X *= 0.97f;

			Projectile.frameCounter++;
			int framespersecond = 10;
			if(Projectile.frameCounter >= (60/ framespersecond))
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}

			if (Projectile.timeLeft < PulseTime)
				Projectile.scale = 1f + (float)Math.Sin(Math.Pow(Projectile.timeLeft / PulseTime, 0.5) * MathHelper.Pi * 6)/4;

			if (!Main.dedServ)
			{
				if(Projectile.velocity.Length() > 6 && Projectile.timeLeft % 4 == 3)
					ParticleHandler.SpawnParticle(new PulseCircle(Projectile.Center, Color.Cyan, 18, 18) { Angle = Projectile.velocity.ToRotation(), ZRotation = 0.5f });

				if(Projectile.timeLeft % 6 == 5)
					ParticleHandler.SpawnParticle(new PulseCircle(Projectile, Color.Cyan * 0.5f, 20, 20));
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Bounce(oldVelocity, 0.4f);
			return false;
		}

		public override int ExplosionRange => 150;

		public override void ExplodeEffect()
		{
			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BassBoom").WithVolume(0.7f) with { PitchVariance = 0.2f }, Projectile.Center);

			for (int i = 1; i <= 3; i++)
				ParticleHandler.SpawnParticle(new PulseCircle(Projectile.Center, Color.Cyan * 0.5f, (ExplosionRange / 3) * i, 15, PulseCircle.MovementType.OutwardsQuadratic));

			for(int i = 0; i < 10; i++)
			{
				Vector2 offset = Main.rand.NextVector2Circular(ExplosionRange / 12, ExplosionRange / 12);
				ParticleHandler.SpawnParticle(new PulseCircle(Projectile.Center + offset, Color.Cyan * 0.7f, (ExplosionRange / 6), Main.rand.Next(14, 22))
				{ Velocity = Vector2.Normalize(offset) * Main.rand.NextFloat(1f, 1.5f), Angle = offset.ToRotation() });
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(Main.spriteBatch);
			Projectile.QuickDrawGlow(Main.spriteBatch);
			if (Projectile.timeLeft < PulseTime)
			{
				float Opacity = (float)Math.Pow((PulseTime - Projectile.timeLeft) / PulseTime, 2);
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), Color.Cyan * Opacity, Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusArrow : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Extra_89";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Arrow");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 70;
			projectile.ranged = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.extraUpdates = 1;
			projectile.arrow = true;
		}

		private ref float _hitvisualtimer => ref projectile.localAI[0];

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(Color.White * 0.2f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 60, 800);
			tM.CreateTrail(projectile, new StandardColorTrail(Color.White), new NoCap(), new DefaultTrailPosition(), 10, 600);
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(101, 255, 245)), new NoCap(), new DefaultTrailPosition(), 120, 500, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.dedServ)
				return true;

			for (int i = 0; i < 8; i++)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, -oldVelocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.75f), Color.White, Color.Cyan, Main.rand.NextFloat(0.2f, 0.3f), 25));

			for (int i = 0; i < 4; i++)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.66f), Color.White, Color.Cyan, Main.rand.NextFloat(0.2f, 0.3f), 25));

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => OnHitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => OnHitEffects();

		public override void AI()
		{
			_hitvisualtimer = Math.Max(_hitvisualtimer - 1, 0); 

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f), 
					Color.White, Color.Cyan, Main.rand.NextFloat(0.12f, 0.18f), 25));
		}

		private void OnHitEffects()
		{
			if (Main.dedServ || _hitvisualtimer > 0)
				return;

			_hitvisualtimer = 3;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = 0; j < 2; j++) //smaller lines
				{
					ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center,
						projectile.velocity.RotatedBy(MathHelper.PiOver2 * i).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(0.1f, 0.2f),
						Color.White, new Vector2(0.25f, Main.rand.NextFloat(1f, 1.5f)), 15));
				}

				//larger blueish lines with less randomization
				ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center,
					projectile.velocity.RotatedBy(MathHelper.PiOver2 * i).RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.15f, 0.2f),
					Color.Lerp(Color.White, Color.Cyan, 0.5f), new Vector2(0.25f, Main.rand.NextFloat(3f, 4f)), 15));
			}

			//impact star particles
			ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Vector2.Zero, Color.Lerp(Color.White, Color.Cyan, 0.5f), Main.rand.NextFloat(0.5f, 0.6f), 12));

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);

			for (int i = 0; i < 5; i++)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(), Color.White, Color.Cyan, Main.rand.NextFloat(0.15f, 0.25f), 20));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.7f, 2f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.7f, 4f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.7f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor, projectile.velocity.ToRotation() + MathHelper.PiOver2, tex.Size() / 2, new Vector2(0.4f, 4f), SpriteEffects.None, 0);
			return false;
		}
	}
}
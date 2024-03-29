﻿using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using SpiritMod.Dusts;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class FlakeRocketProj : BaseProj.BaseRocketProj, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Flake Rocket");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.RocketI);
			projectile.width = 26;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.penetrate = 1;
			projectile.ranged = true;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 240;
		}
		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new GradientTrail(Color.Lerp(Color.Cyan, Color.White, 0.7f) * 0.2f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 30, 100);

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			if (!Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(),
					new Color(135, 253, 255), new Color(0, 21, 255), Main.rand.NextFloat(0.35f, 0.5f), 6, delegate (Particle particle)
					{
						particle.Velocity *= 0.94f;
					}));

				if (Main.rand.NextBool(3))
					ParticleHandler.SpawnParticle(new SmokeParticle(projectile.Center, -projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.1f), new Color(30, 30, 90) * 0.5f, Main.rand.NextFloat(0.2f, 0.3f), 12));
			}
		}

		public override void AbstractHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(ModContent.BuffType<CryoCrush>(), 300, true);

		public override void ExplodeEffect()
		{
			Main.PlaySound(new LegacySoundStyle(soundId: SoundID.Item, style: 14).WithPitchVariance(0.1f), projectile.Center);
			DustHelper.DrawDustImage(projectile.Center, ModContent.DustType<WinterbornDust>(), 0.3f, "SpiritMod/Effects/Snowflakes/Flake" + Main.rand.Next(3), 0.4f);
			float rot = Main.rand.NextFloat(MathHelper.TwoPi);
			for (int i = 0; i < 8; i++)
				DustHelper.DrawDustImage(projectile.Center + (Vector2.UnitX.RotatedBy(rot + (MathHelper.TwoPi * i / 8f)) * 60), ModContent.DustType<WinterbornDust>(), 0.15f, "SpiritMod/Effects/Snowflakes/Flake" + Main.rand.Next(3), 0.25f, rot: Main.rand.NextFloat(MathHelper.TwoPi));

			for (int i = 0; i < 30; i++)
			{
				float maxDist = 100;
				float Dist = Main.rand.NextFloat(maxDist);
				Vector2 offset = Main.rand.NextVector2Unit();
				ParticleHandler.SpawnParticle(new SmokeParticle(projectile.Center + (offset * Dist), Main.rand.NextFloat(5f) * offset * (1 - (Dist / maxDist)), new Color(30, 30, 90) * 0.5f, Main.rand.NextFloat(0.4f, 0.6f), 40));
			}

			for (int i = 0; i < 8; i++)
				ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, (projectile.velocity / 4) - (Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 3) * Main.rand.NextFloat(4)),
					new Color(135, 253, 255), new Color(0, 21, 255), Main.rand.NextFloat(0.5f, 0.7f), 40, delegate (Particle particle)
					{
						if (particle.Velocity.Y < 16)
							particle.Velocity.Y += 0.12f;
					}));
		}
	}
}

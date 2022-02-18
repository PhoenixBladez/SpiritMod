using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles
{
	public class MeteorCastProjectile : ModProjectile, ITrailProjectile//, IManualTrailProjectile, IDrawAdditive
	{
		private static Color Yellow = new Color(242, 240, 134);
		private static Color Orange = new Color(255, 98, 74);

		internal Vector2 Destination => new Vector2(projectile.ai[0], projectile.ai[1]);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 32);
			projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			projectile.hostile = true;
			projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(Yellow, Orange), new RoundCap(), new DefaultTrailPosition(), 100f * projectile.scale, 220f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new GradientTrail(Yellow, Orange), new RoundCap(), new DefaultTrailPosition(), 120f * projectile.scale, 220f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(Color.Orange * 0.5f), new RoundCap(), new DefaultTrailPosition(), 10f * projectile.scale, 90f * projectile.scale, new DefaultShader());
		}

		public override void AI()
		{
			float intensity = .001f;
			Lighting.AddLight(projectile.position, Orange.R * intensity, Orange.G * intensity, Orange.B * intensity);

			if (projectile.DistanceSQ(Destination) < 20 * 20)
				projectile.Kill();

			if (!Main.dedServ)
				MakeEmberParticle(projectile.velocity * 0.5f, 0.97f);
		}

		private void MakeEmberParticle(Vector2 vel, float velDecayRate)
		{
			ParticleHandler.SpawnParticle(new FireParticle(projectile.Center + Main.rand.NextVector2Circular(10, 10) * projectile.scale,
				vel, Yellow, Orange, Main.rand.NextFloat(0.2f, 0.4f), 35, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i)
				Projectile.NewProjectile(projectile.Center, new Vector2(0, Main.rand.NextFloat(7f, 14f)).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<MeteorEnchantment_Meteor>(), 20, 1f);

			Mechanics.EventSystem.EventManager.PlayEvent(new Mechanics.EventSystem.Events.ScreenShake(6f, 0.6f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], Destination - Main.screenPosition, null, lightColor * 0.4f);
			return true;
		}
	}
}
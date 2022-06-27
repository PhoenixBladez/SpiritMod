using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles
{
	public class MeteorEnchantment_Meteor : ModProjectile, ITrailProjectile//, IManualTrailProjectile, IDrawAdditive
	{
		private static Color Yellow = new Color(242, 240, 134);
		private static Color Orange = new Color(255, 98, 74);

		public int visualTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 32);
			Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			Projectile.hostile = true;
			Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new GradientTrail(Yellow, Orange), new RoundCap(), new DefaultTrailPosition(), 100f * Projectile.scale, 220f * Projectile.scale, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new GradientTrail(Yellow, Orange), new RoundCap(), new DefaultTrailPosition(), 120f * Projectile.scale, 220f * Projectile.scale, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new StandardColorTrail(Color.Orange * 0.5f), new RoundCap(), new DefaultTrailPosition(), 10f * Projectile.scale, 90f * Projectile.scale, new DefaultShader());
		}

		public override void AI()
		{
			float intensity = .001f;
			Lighting.AddLight(Projectile.position, Orange.R * intensity, Orange.G * intensity, Orange.B * intensity);

			Projectile.velocity *= 1.004f;

			if (!Main.dedServ)
				MakeEmberParticle(Projectile.velocity * 0.5f, 0.97f);
		}

		private void MakeEmberParticle(Vector2 vel, float velDecayRate)
		{
			ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center + Main.rand.NextVector2Circular(10, 10) * Projectile.scale,
				vel, Yellow, Orange, Main.rand.NextFloat(0.2f, 0.4f), 35, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D Mask = ModContent.Request<Texture2D>(Texture + "_Glow");
			Projectile.QuickDraw(spriteBatch);
			void DrawGlow(Vector2 positionOffset, Color Color) =>
				spriteBatch.Draw(Mask, Projectile.Center - Main.screenPosition + positionOffset, Projectile.DrawFrame(), Color, Projectile.rotation,
				Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);

			Color additiveWhite = Color.White;
			additiveWhite.A = 0;

			DrawGlow(Vector2.Zero, Color.White);
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 6, delegate (Vector2 offset, float opacity)
			{
				DrawGlow(offset, additiveWhite * opacity * 0.5f);
			});
			DrawGlow(Vector2.Zero, additiveWhite);
			return false;
		}
	}
}
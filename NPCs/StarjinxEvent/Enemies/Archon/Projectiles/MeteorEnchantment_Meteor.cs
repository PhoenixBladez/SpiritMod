using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;
using System.IO;
using SpiritMod.Mechanics.Trails.CustomTrails;

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
			Main.projFrames[projectile.type] = 6;
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

			projectile.velocity *= 1.004f;

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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D Mask = ModContent.GetTexture(Texture + "_Glow");
			projectile.QuickDraw(spriteBatch);
			void DrawGlow(Vector2 positionOffset, Color Color) =>
				spriteBatch.Draw(Mask, projectile.Center - Main.screenPosition + positionOffset, projectile.DrawFrame(), Color, projectile.rotation,
				projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);

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
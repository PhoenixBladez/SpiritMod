using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Occultist
{
	public class OccultistHand : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Dark Grasp");

		private readonly int maxTimeLeft = 180;
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.hostile = true;
			projectile.scale = Main.rand.NextFloat(0.12f, 0.2f);
			projectile.timeLeft = maxTimeLeft;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.spriteDirection = Main.rand.NextBool() ? -1 : 1;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51, 150) * 0.1f), new RoundCap(), new ArrowGlowPosition(), 200 * projectile.scale, 100);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51, 150), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * projectile.scale, 60, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51, 150), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * projectile.scale, 60, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.2f, 1f, 1f));
		}

		private readonly float period = 80;
		private readonly float twinkleTime = 15;
		private ref float Amplitude => ref projectile.ai[0];
		private ref float PeriodOffset => ref projectile.ai[1];

		private float AiTimer => maxTimeLeft - projectile.timeLeft;

		public override void AI()
		{
			projectile.position -= projectile.velocity;
			if(AiTimer > twinkleTime)
			{
				Vector2 cosVel = projectile.velocity.RotatedBy(MathHelper.ToRadians(Amplitude) * (float)Math.Sin(((AiTimer + PeriodOffset) / period) * MathHelper.TwoPi));
				projectile.position += cosVel;
				projectile.rotation = cosVel.ToRotation() + MathHelper.PiOver2;

				if(Main.rand.NextBool(10) && !Main.dedServ)
					ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

				if (projectile.alpha > 0)
				{
					projectile.alpha -= 25;
					projectile.scale *= 1.15f;
					projectile.velocity *= 1.15f;
				}
				else
					projectile.alpha = 0;

				if (AiTimer > maxTimeLeft / 3f)
				{
					projectile.tileCollide = true;
					if(projectile.velocity.Length() < 20)
						projectile.velocity *= 1.015f;
				}
			}
		}

		//public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Obstructed, 40, true);

		public override void Kill(int timeLeft)
		{
			if (Main.dedServ)
				return;

			Main.PlaySound(SoundID.DD2_SkeletonHurt.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);
			for (int i = 0; i < 10; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center + Main.rand.NextVector2Circular(20, 10),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(2), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

			for (int j = 0; j < 6; j++)
				Gore.NewGore(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(), mod.GetGoreSlot("Gores/Skelet/grave" + Main.rand.Next(1, 5)), 0.5f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(AiTimer < twinkleTime)
			{
				Texture2D twinkle = Main.extraTexture[89];

				float halftwinkletime = twinkleTime / 2f;
				float opacity = 0.7f * (halftwinkletime - Math.Abs(halftwinkletime - AiTimer)) / halftwinkletime;
				float rotation = (AiTimer / twinkleTime) * projectile.spriteDirection * MathHelper.TwoPi;

				void DrawTex(float AdditionalRotation, float ScaleMod, Color BaseColor)
				{
					spriteBatch.Draw(twinkle, projectile.Center - Main.screenPosition, null, BaseColor * opacity, rotation + AdditionalRotation, twinkle.Size() / 2, opacity * ScaleMod, SpriteEffects.None, 0);
				}
				DrawTex(0f, 0.75f, new Color(252, 68, 166, 100));
				DrawTex(MathHelper.PiOver2, 0.75f, new Color(252, 68, 166, 100));

				DrawTex(MathHelper.PiOver4, 0.33f, Color.White * 0.5f);
				DrawTex(3 * MathHelper.PiOver4, 0.33f, Color.White * 0.5f);
			}

			Texture2D maskTex = ModContent.GetTexture(Texture + "_mask");
			spriteBatch.Draw(maskTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(99, 23, 51) * Math.Max(1 - ((AiTimer - twinkleTime) / 60f), 0)), projectile.rotation, 
				maskTex.Size() / 2, projectile.scale, (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
		}
	}
}
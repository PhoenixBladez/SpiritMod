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

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus_Meteor : ModProjectile, IManualTrailProjectile, IDrawAdditive
	{
		private const int TELEGRAPH_TIME = 0; //Time for the projectile's path to be fully telegraphed, and the minimum time for the projectile to fall downwards (temporarily set to 0)

		private static Color Yellow = new Color(242, 240, 134);
		private static Color Orange = new Color(255, 98, 74);
		private static Color Purple = new Color(255, 0, 144);

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
			projectile.alpha = 255;
			projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateCustomTrail(new FlameTrail(projectile, Yellow, Orange, Purple, 40 * projectile.scale, 15));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Yellow * 0.33f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 80 * projectile.scale, 300, null, TrailLayer.AboveProjectile);
		}

		private ref float Timer => ref projectile.ai[0];
		private ref float Delay => ref projectile.ai[1];

		private int FallStartTime => TELEGRAPH_TIME + (int)Delay; //Time when the projectile becomes active and starts falling

		public override bool CanDamage() => Timer < FallStartTime;

		public override void AI()
		{
			int fadeTime = 60; //Time in ticks to fully fade in
			projectile.rotation += (Math.Sign(projectile.velocity.X) > 0 ? 1 : -1) * 0.12f;

			if (++Timer >= FallStartTime)
			{
				if(!Main.dedServ)
				{
					if (Timer == FallStartTime) //Spawn trail on first tick, add sound here later?
						TrailManager.ManualTrailSpawn(projectile);

					MakeEmberParticle(projectile.velocity * 0.5f, 0.97f);
				}

				projectile.alpha = Math.Max(projectile.alpha - (255 / fadeTime), 0);

				if ((Timer - FallStartTime) < 90)
					projectile.velocity *= 1.02f;
			}
			else
				projectile.position -= projectile.velocity; //Stay in place until falling starts
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Timer);
			writer.Write(projectile.frame);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Timer = reader.ReadSingle();
			projectile.frame = reader.ReadInt32();
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

		public void AdditiveCall(SpriteBatch sb)
		{
			/*float progress = (Timer - Delay) / TELEGRAPH_TIME; 
			progress = MathHelper.Clamp(progress, 0, 1);

			Texture2D telegraphTex = mod.GetTexture("Textures/GlowTrail");

			Vector2 scale = new Vector2(3000 / telegraphTex.Width, MathHelper.Lerp(120, 15, EaseFunction.EaseCubicOut.Ease(progress)) / telegraphTex.Height);
			float opacity = EaseFunction.EaseCubicOut.Ease(progress) * EaseFunction.EaseQuinticIn.Ease(1 - projectile.Opacity) * 0.75f;
			Vector2 origin = new Vector2(0, telegraphTex.Height / 2);

			float ColorLerp = EaseFunction.EaseCubicOut.Ease(progress);
			Color color;
			if (ColorLerp < 0.5f)
				color = Color.Lerp(Purple, Orange, ColorLerp * 2);
			else
				color = Color.Lerp(Orange, Yellow, (ColorLerp - 0.5f) * 2);

			sb.Draw(telegraphTex, projectile.Center - Main.screenPosition, null, color * opacity, projectile.velocity.ToRotation(), origin, scale, SpriteEffects.None, 0);*/
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);

				for (int i = 0; i < 8; i++)
					MakeEmberParticle(-projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f), 0.96f);

				for (int i = 0; i < 6; i++)
					MakeEmberParticle(projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.33f), 0.96f);
			}
		}

		private void MakeEmberParticle(Vector2 vel, float velDecayRate)
		{
			ParticleHandler.SpawnParticle(new FireParticle(projectile.Center + Main.rand.NextVector2Circular(10, 10) * projectile.scale,
				vel, Yellow, Orange, Main.rand.NextFloat(0.2f, 0.4f), 35, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}
	}
}
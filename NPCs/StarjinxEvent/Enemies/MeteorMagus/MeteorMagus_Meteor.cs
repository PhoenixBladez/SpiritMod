using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 32);
			Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateCustomTrail(new FlameTrail(Projectile, Yellow, Orange, Purple, 40 * Projectile.scale, 15));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Yellow * 0.33f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 80 * Projectile.scale, 300, null, TrailLayer.AboveProjectile);
		}

		private ref float Timer => ref Projectile.ai[0];
		private ref float Delay => ref Projectile.ai[1];

		private int FallStartTime => TELEGRAPH_TIME + (int)Delay; //Time when the projectile becomes active and starts falling

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Timer < FallStartTime;

		public override void AI()
		{
			int fadeTime = 60; //Time in ticks to fully fade in
			Projectile.rotation += (Math.Sign(Projectile.velocity.X) > 0 ? 1 : -1) * 0.12f;

			if (++Timer >= FallStartTime)
			{
				if(!Main.dedServ)
				{
					if (Timer == FallStartTime) //Spawn trail on first tick, add sound here later?
						TrailManager.ManualTrailSpawn(Projectile);

					MakeEmberParticle(Projectile.velocity * 0.5f, 0.97f);
				}

				Projectile.alpha = Math.Max(Projectile.alpha - (255 / fadeTime), 0);

				if ((Timer - FallStartTime) < 90)
					Projectile.velocity *= 1.02f;
			}
			else
				Projectile.position -= Projectile.velocity; //Stay in place until falling starts
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Timer);
			writer.Write(Projectile.frame);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Timer = reader.ReadSingle();
			Projectile.frame = reader.ReadInt32();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D Mask = ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.QuickDraw(Main.spriteBatch);
			void DrawGlow(Vector2 positionOffset, Color Color) => 
				Main.spriteBatch.Draw(Mask, Projectile.Center - Main.screenPosition + positionOffset, Projectile.DrawFrame(), Color, Projectile.rotation, 
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
				SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.65f, PitchVariance = 0.3f }, Projectile.Center);

				for (int i = 0; i < 8; i++)
					MakeEmberParticle(-Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f), 0.96f);

				for (int i = 0; i < 6; i++)
					MakeEmberParticle(Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.33f), 0.96f);
			}
		}

		private void MakeEmberParticle(Vector2 vel, float velDecayRate)
		{
			ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center + Main.rand.NextVector2Circular(10, 10) * Projectile.scale,
				vel, Yellow, Orange, Main.rand.NextFloat(0.2f, 0.4f), 35, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}
	}
}
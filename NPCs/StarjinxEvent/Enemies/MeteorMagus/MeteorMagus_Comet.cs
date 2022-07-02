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
	public class MeteorMagus_Comet : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override string Texture => "SpiritMod/NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_Meteor";

		private const float GRAVITY = 0.3f;
		private const float PRE_LAUNCH_TIME = 30;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 32);
			Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.scale = Main.rand.NextFloat(0.66f, 0.8f);
			Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateCustomTrail(new FlameTrail(Projectile, Color.Cyan, Color.DarkCyan, Color.DarkBlue, 20, 10));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Cyan * 0.33f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 60 * Projectile.scale, 300, null, TrailLayer.AboveProjectile);
		}

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];
		private Player Target => Main.player[(int)Projectile.ai[1]];

		private ref float Timer => ref Projectile.localAI[0];

		public override bool PreAI()
		{
			if (!Main.dedServ && Main.rand.NextBool())
				MakeEmberParticle(Projectile.velocity / 2, 0.97f);

			return true;
		}
		public override void AI()
		{
			Projectile.alpha = Math.Max(Projectile.alpha - 15, 0);

			Projectile.tileCollide = Timer > (PRE_LAUNCH_TIME + 60); //Arbitrarily make it not collide with tile until a second after it launches, to make attack function better
			Projectile.rotation += Projectile.velocity.X * 0.05f;

			if (Timer < PRE_LAUNCH_TIME)
			{
				Projectile.velocity = -Vector2.Normalize(Projectile.GetArcVel(Target.Center, GRAVITY, 300, maxXvel: 12, heightabovetarget: 100)) * 2f;
				if (!Parent.active || !Target.active || Target.dead)
					Projectile.Kill();
			}
			else if (Timer > PRE_LAUNCH_TIME)
				Projectile.velocity.Y += GRAVITY;

			if (++Timer == PRE_LAUNCH_TIME)
				Projectile.velocity = Projectile.GetArcVel(Target.Center, GRAVITY, 300, maxXvel : 12, heightabovetarget: 100).RotatedByRandom(MathHelper.Pi / 12);

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
			Texture2D BlueMask = ModContent.Request<Texture2D>(Texture + "_BlueGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.QuickDraw(Main.spriteBatch);
			void DrawBlueGlow(Vector2 positionOffset, Color Color) =>
				Main.spriteBatch.Draw(BlueMask, Projectile.Center - Main.screenPosition + positionOffset, Projectile.DrawFrame(), Color, Projectile.rotation, 
				Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);

			Color additiveWhite = Color.White;
			additiveWhite.A = 0;
			DrawBlueGlow(Vector2.Zero, Color.White);
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 6, delegate (Vector2 offset, float opacity)
			{
				DrawBlueGlow(offset, additiveWhite * opacity * 0.5f);
			});
			DrawBlueGlow(Vector2.Zero, additiveWhite);


			return false;
		}

		public void AdditiveCall(SpriteBatch sb)
		{
			Texture2D WhiteMask = ModContent.Request<Texture2D>(Texture + "_Mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			float whiteMaskOpacity = 1 - (Timer / PRE_LAUNCH_TIME);
			whiteMaskOpacity = Math.Max(whiteMaskOpacity, 0);
			whiteMaskOpacity = EaseFunction.EaseQuadOut.Ease(whiteMaskOpacity);

			sb.Draw(WhiteMask, Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), Color.White * whiteMaskOpacity * Projectile.Opacity, Projectile.rotation,
				Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);

			float blurLength = 200 * Projectile.scale * whiteMaskOpacity * Projectile.Opacity;
			float blurWidth = 25 * Projectile.scale * whiteMaskOpacity * Projectile.Opacity;

			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = Projectile.Center - Main.screenPosition,
				Height = blurWidth,
				Length = blurLength,
				Rotation = 0,
				Color = Color.White * whiteMaskOpacity * Projectile.Opacity
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/starHit") with { Volume = 0.65f, PitchVariance = 0.3f }, Projectile.Center);

				for (int i = 0; i < 8; i++)
					MakeEmberParticle(-Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f), 0.96f);

				for (int i = 0; i < 6; i++)
					MakeEmberParticle(Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.33f), 0.96f);
			}
		}

		private void MakeEmberParticle(Vector2 vel, float velDecayRate)
		{
			ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center + Main.rand.NextVector2Circular(10, 10) * Projectile.scale,
				vel, Color.LightCyan, Color.Cyan, Main.rand.NextFloat(0.2f, 0.4f), 25, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}
	}
}
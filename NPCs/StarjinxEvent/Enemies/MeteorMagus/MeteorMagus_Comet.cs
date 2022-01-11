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
	public class MeteorMagus_Comet : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override string Texture => "SpiritMod/NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_Meteor";

		private const float GRAVITY = 0.3f;
		private const float PRE_LAUNCH_TIME = 30;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 32);
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(0.66f, 0.8f);
			projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateCustomTrail(new FlameTrail(projectile, Color.Cyan, Color.DarkCyan, Color.DarkBlue, 20, 10));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Cyan * 0.33f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 60 * projectile.scale, 300, null, TrailLayer.AboveProjectile);
		}

		private NPC Parent => Main.npc[(int)projectile.ai[0]];
		private Player Target => Main.player[(int)projectile.ai[1]];

		private ref float Timer => ref projectile.localAI[0];

		public override bool PreAI()
		{
			if (!Main.dedServ && Main.rand.NextBool())
				MakeEmberParticle(projectile.velocity / 2, 0.97f);

			return true;
		}
		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);

			projectile.tileCollide = Timer > (PRE_LAUNCH_TIME + 60); //Arbitrarily make it not collide with tile until a second after it launches, to make attack function better
			projectile.rotation += projectile.velocity.X * 0.05f;

			if (Timer < PRE_LAUNCH_TIME)
			{
				projectile.velocity = -Vector2.Normalize(projectile.GetArcVel(Target.Center, GRAVITY, 300, maxXvel: 12, heightabovetarget: 100)) * 2f;
				if (!Parent.active || !Target.active || Target.dead)
					projectile.Kill();
			}
			else if (Timer > PRE_LAUNCH_TIME)
				projectile.velocity.Y += GRAVITY;

			if (++Timer == PRE_LAUNCH_TIME)
				projectile.velocity = projectile.GetArcVel(Target.Center, GRAVITY, 300, maxXvel : 12, heightabovetarget: 100).RotatedByRandom(MathHelper.Pi / 12);

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
			Texture2D BlueMask = ModContent.GetTexture(Texture + "_BlueGlow");
			projectile.QuickDraw(spriteBatch);
			void DrawBlueGlow(Vector2 positionOffset, Color Color) => 
				spriteBatch.Draw(BlueMask, projectile.Center - Main.screenPosition + positionOffset, projectile.DrawFrame(), Color, projectile.rotation, 
				projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);

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
			Texture2D WhiteMask = ModContent.GetTexture(Texture + "_Mask");
			float whiteMaskOpacity = 1 - (Timer / PRE_LAUNCH_TIME);
			whiteMaskOpacity = Math.Max(whiteMaskOpacity, 0);
			whiteMaskOpacity = EaseFunction.EaseQuadOut.Ease(whiteMaskOpacity);

			sb.Draw(WhiteMask, projectile.Center - Main.screenPosition, projectile.DrawFrame(), Color.White * whiteMaskOpacity * projectile.Opacity, projectile.rotation,
				projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);

			float blurLength = 200 * projectile.scale * whiteMaskOpacity * projectile.Opacity;
			float blurWidth = 25 * projectile.scale * whiteMaskOpacity * projectile.Opacity;

			Effect blurEffect = mod.GetEffect("Effects/BlurLine");
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = projectile.Center - Main.screenPosition,
				Height = blurWidth,
				Length = blurLength,
				Rotation = 0,
				Color = Color.White * whiteMaskOpacity * projectile.Opacity
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
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
				vel, Color.LightCyan, Color.Cyan, Main.rand.NextFloat(0.2f, 0.4f), 25, delegate (Particle p)
				{
					p.Velocity *= velDecayRate;
				}));
		}
	}
}
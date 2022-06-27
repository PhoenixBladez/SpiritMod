using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.BowsMisc.Carrion
{
	public class CarrionCrowArrow : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrion Crow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.alpha = 255;
			Projectile.timeLeft = 900;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new CarrionCrowTrail(Projectile, new Color(99, 23, 51, 150)), new RoundCap(), new DefaultTrailPosition(), 90f, 180f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			//tManager.CreateTrail(projectile, new CarrionCrowTrail(projectile, Color.White * 0.2f), new NoCap(), new DefaultTrailPosition(), 24f, 80f, new DefaultShader());
		}

		internal class CarrionCrowTrail : ITrailColor
		{
			private Color _colour;
			private Projectile _proj;

			public CarrionCrowTrail(Projectile projectile, Color colour)
			{
				_colour = colour;
				_proj = projectile;
			}

			public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
			{
				float progress = distanceFromStart / trailLength;
				return _colour * (1f - progress) * MathHelper.Lerp(0f, 1, _proj.localAI[0]);
			}
		}

		public bool looping = false;
		public int loopSize = 14;
		public int loopCounter = 0;
		public override void AI()
        {
			Projectile.alpha = Math.Max(Projectile.alpha - 10, 0);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.frameCounter++;

			if (Projectile.frameCounter >= 3)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}
			if (looping)
			{
				Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
				Projectile.velocity = currentSpeed.RotatedBy(Math.PI / loopSize);
				loopCounter++;
				Projectile.tileCollide = false;
				if (loopCounter >= loopSize * 2)
				{
					looping = false;
					Projectile.tileCollide = true;
				}
			}
			if (Projectile.penetrate < 4 && !looping)
            {
				Homing();
            }
			if (Projectile.penetrate <= 4)
			{
				if (Projectile.localAI[0] < 1.5f)
				{
					Projectile.localAI[0] += .075f;
				}
			}
			else if (Projectile.velocity.Length() < 24)
				Projectile.velocity *= 1.015f;
		}

		public void Homing()
        {
			bool chasing = true;

			NPC target = null;
			if (Projectile.ai[0] == -1f)
			{
				target = ProjectileExtras.FindRandomNPC(Projectile.Center, 1000f, false);
			}
			else
			{
				target = Main.npc[(int)Projectile.ai[0]];
				if (!target.active || !target.CanBeChasedBy())
				{
					target = ProjectileExtras.FindRandomNPC(Projectile.Center, 1000f, false);
				}
			}

			if (target == null)
			{
				chasing = false;
				Projectile.ai[0] = -1f;
			}
			else
			{
				Projectile.tileCollide = false;
				Projectile.ai[0] = target.whoAmI;
				if (Math.Abs(MathHelper.WrapAngle(Projectile.velocity.ToRotation() - Projectile.AngleTo(target.Center))) < (MathHelper.Pi / 6f)) //if close enough in desired angle, accelerate and home accurately
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 26, 0.175f);

				else //if too much of an angle, circle around
				{
					if (Projectile.velocity.Length() > 8)
						Projectile.velocity *= 0.96f;

					if (Projectile.velocity.Length() < 6)
						Projectile.velocity *= 1.05f;

					Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Projectile.velocity.Length(), 0.15f));
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing)
			{
				Projectile.tileCollide = false;
				Vector2 dir = Projectile.velocity;
				float vel = Projectile.velocity.Length();
				if (vel != 0f)
				{
					if (vel < 4.8f)
					{
						dir *= 1 / vel;
						Projectile.velocity += dir * 0.0625f;
					}
				}
				else
				{
					//Stops the projectiles from spazzing out
					Projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(new LegacySoundStyle(29, 53).WithPitchVariance(0.3f), Projectile.Center);
			if (Projectile.penetrate <= 4)
			{
				for (int num257 = 0; num257 < 20; num257++)
					Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(4, 4), 134, 
						Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f, 0.6f), 0, default, Main.rand.NextFloat(1f, 1.5f)).noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.penetrate > 4)
            {
				SoundEngine.PlaySound(new LegacySoundStyle(4, 6).WithPitchVariance(0.3f), Projectile.Center);
			}
			if (Projectile.penetrate > 4 || Projectile.penetrate == 3)
			{ 
				looping = true;
				loopSize = (int)(Main.rand.Next(60, 63)/Projectile.penetrate);
				Projectile.velocity *= 1.4f;
			}
			if (Projectile.penetrate <= 4)
            {
				Projectile.localAI[0] += .38f;
            }

			Projectile.ai[0] = target.whoAmI;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = Color.Black * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * Projectile.Opacity;

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Items/Sets/BowsMisc/Carrion/CarrionCrowArrow_Glow");
				Texture2D tex2 = ModContent.Request<Texture2D>("SpiritMod/Items/Sets/BowsMisc/Carrion/CarrionCrowArrowEye");

				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);

				Color color1 = new Color(255, 186, 252) * Projectile.Opacity * 0.45475f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				for (int j = 0; j < 3; j++)
				{
					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, Projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
				}
				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * .3f * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
				spriteBatch.Draw(tex2, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * .5f * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
			}
		}
	}
}

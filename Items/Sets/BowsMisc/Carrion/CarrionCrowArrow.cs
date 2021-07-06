using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.BowsMisc.Carrion
{
	public class CarrionCrowArrow : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrion Crow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.width = 22;
			projectile.height = 22;
			projectile.alpha = 255;
			projectile.timeLeft = 900;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new CarrionCrowTrail(projectile, new Color(99, 23, 51, 200)), new RoundCap(), new DefaultTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
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
			projectile.alpha = Math.Max(projectile.alpha - 10, 0);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			if (looping)
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Math.PI / loopSize);
				loopCounter++;
				if (loopCounter >= loopSize * 2)
				{
					looping = false;
				}
			}
			if (projectile.penetrate < 4 && !looping)
            {
				Homing();
            }
			if (projectile.penetrate <= 4)
            {
				if (projectile.localAI[0] < 1.5f)
				{
					projectile.localAI[0] += .15f;
				}
            }
		}

		public void Homing()
        {
			projectile.ai[1] += 1f;
			bool chasing = false;
			if (projectile.ai[1] >= 20f)
			{
				chasing = true;

				projectile.friendly = true;
				NPC target = null;
				if (projectile.ai[0] == -1f)
				{
					target = ProjectileExtras.FindRandomNPC(projectile.Center, 1360f, false);
				}
				else
				{
					target = Main.npc[(int)projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy())
					{
						target = ProjectileExtras.FindRandomNPC(projectile.Center, 1360f, false);
					}
				}

				if (target == null)
				{
					chasing = false;
					projectile.ai[0] = -1f;
				}
				else
				{
					projectile.ai[0] = (float)target.whoAmI;
					if (projectile.Distance(target.Center) > 150)
					{
						if (projectile.velocity.Length() < 20f)
							projectile.velocity *= 1.04f;
						projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * projectile.velocity.Length(), 0.2f));
					}
					else if (projectile.Distance(target.Center) > 20)
						projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 12, 0.07f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing)
			{
				Vector2 dir = projectile.velocity;
				float vel = projectile.velocity.Length();
				if (vel != 0f)
				{
					if (vel < 4.8f)
					{
						dir *= 1 / vel;
						projectile.velocity += dir * 0.0625f;
					}
				}
				else
				{
					//Stops the projectiles from spazzing out
					projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(29, 53).WithPitchVariance(0.3f), projectile.Center);
			if (projectile.penetrate <= 4)
			{
				Vector2 vector9 = projectile.position;
				Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
				vector9 += value19 * 16f;
				for (int num257 = 0; num257 < 20; num257++)
				{
					int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 134, 0f, 0f, 0, default(Color), 1f);
					Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
					Main.dust[newDust].velocity += value19 * 2f;
					Main.dust[newDust].velocity *= 0.5f;
					Main.dust[newDust].noGravity = true;
					vector9 -= value19 * 8f;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.penetrate > 4)
            {
				Main.PlaySound(new LegacySoundStyle(4, 6).WithPitchVariance(0.3f), projectile.Center);
			}
			if (projectile.penetrate > 4 || projectile.penetrate == 3)
			{ 
				looping = true;
				loopSize = (int)(Main.rand.Next(60, 63)/projectile.penetrate);
				projectile.velocity *= 1.4f;
			}
			if (projectile.penetrate <= 4)
            {
				projectile.localAI[0] += .38f;
            }
			projectile.tileCollide = false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = Color.Black * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * projectile.Opacity;

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Sets/BowsMisc/Carrion/CarrionCrowArrow_Glow");
				Texture2D tex2 = ModContent.GetTexture("SpiritMod/Items/Sets/BowsMisc/Carrion/CarrionCrowArrowEye");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);

				Color color1 = new Color(255, 186, 252) * projectile.Opacity * 0.45475f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				for (int j = 0; j < 3; j++)
				{
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
				}
				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, Color.White * .3f * projectile.Opacity, projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
				spriteBatch.Draw(tex2, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, Color.White * .5f * projectile.Opacity, projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
			}
		}
	}
}

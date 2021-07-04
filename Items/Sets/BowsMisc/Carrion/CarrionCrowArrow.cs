using Terraria;
using Terraria.ID;
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
			projectile.width = 12;
			projectile.height = 32;
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
				return _colour * (1f - progress) * MathHelper.Lerp(0, 1, 4 - (_proj.penetrate));
			}
		}
		public bool looping = false;
		public int loopSize = 12;
		public int loopCounter = 0;
		public override void AI()
        {
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
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
			if (projectile.penetrate < 3 && !looping)
            {
				Homing();
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
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
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

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 7f)
				vector *= 7f / magnitude;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.penetrate > 3 || projectile.penetrate == 2)
			{ 
				looping = true;
				loopSize = (int)(Main.rand.Next(34, 37)/projectile.penetrate);
				projectile.velocity *= 1.4f;
			}
			projectile.tileCollide = false;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(20, 20, 20) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Sets/BowsMisc/Carrion/CarrionCrowArrow_Glow");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);

				Color color1 = new Color(255, 186, 252) * 0.45475f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				for (int j = 0; j < 3; j++)
				{
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, projectile.rotation, tex.Size() / 2, scale * 1.25425f, default, default);
				}
			}
		}
	}
}

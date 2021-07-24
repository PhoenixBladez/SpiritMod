using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Occultist
{
	public class OccultistSoul : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Lost Soul");

		public override void SetDefaults()
		{
			projectile.timeLeft = 180;
			projectile.hostile = true;
			projectile.height = 24;
			projectile.width = 24;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(0.5f, 0.6f);
			projectile.penetrate = -1;
			projectile.hide = true;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			float scaleMod = projectile.scale * (VisualOnly ? 1.25f : 2f);
			float colorMod = VisualOnly ? 0.33f : 1f;
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(252, 3, 148, 100) * 0.5f * colorMod), new RoundCap(), new DefaultTrailPosition(), 3f * scaleMod, 80f * scaleMod, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(252, 3, 148, 100) * 0.5f * colorMod), new RoundCap(), new DefaultTrailPosition(), 60f * scaleMod, 120f * scaleMod, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));

			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(252, 3, 148, 100) * colorMod), new RoundCap(), new DefaultTrailPosition(), 20f * scaleMod, 80f * (float)Math.Pow(scaleMod, 2));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Lerp(new Color(252, 3, 148, 100), Color.White, 0.75f) * colorMod), new RoundCap(), new DefaultTrailPosition(), 12f * scaleMod, 60f * (float)Math.Pow(scaleMod, 2));
		}

		private bool VisualOnly => projectile.ai[0] == 0;

		private ref float AiState => ref projectile.localAI[0];

		private const int STATE_PASSIVEMOVEMENT = 0;
		private const int STATE_LOCKEDON = 1;
		private const int STATE_ACCELERATE = 2;
		private const int STATE_FADEOUT = 3;

		private const int PASSIVETIME = 40;
		private const int LOCKONTIME = 30;
		private const int ACCELLIFETIME = 160;

		private ref float AiTimer => ref projectile.localAI[1];

		private Player Target => Main.player[(int)projectile.ai[1]];

		public override void AI()
		{
			if(!VisualOnly && (Target.dead || !Target.active))
			{
				projectile.Kill();
				return;
			}
			Lighting.AddLight(projectile.Center, Color.Magenta.ToVector3() * projectile.Opacity * 1.5f);

			if (Main.rand.NextBool(5) && !Main.dedServ)
				Particles.ParticleHandler.SpawnParticle(new Particles.GlowParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.5f), new Color(252, 3, 148, 100) * projectile.Opacity * 1.5f, Main.rand.NextFloat(0.04f, 0.06f) * projectile.scale, 40));

			switch (AiState)
			{
				case STATE_PASSIVEMOVEMENT:
					projectile.alpha = Math.Max(projectile.alpha - 5, 0);
					projectile.velocity *= 0.97f;
					if (Main.rand.NextBool(3))
					{
						projectile.velocity = projectile.velocity.RotatedByRandom(0.4f);
						projectile.netUpdate = true;
					}

					if(AiTimer > PASSIVETIME)
					{
						AiState = (VisualOnly) ? STATE_FADEOUT : STATE_LOCKEDON;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;
				case STATE_LOCKEDON:
					if (projectile.velocity.Length() < 5)
						projectile.velocity *= 1.06f;
					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.08f));
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 5, 0.02f);
					if (AiTimer > LOCKONTIME)
					{
						AiState = STATE_ACCELERATE;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;
				case STATE_ACCELERATE:
					if (projectile.velocity.Length() < 15)
						projectile.velocity *= 1.06f;

					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.03f));
					
					if(AiTimer > ACCELLIFETIME)
					{
						AiState = STATE_FADEOUT;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_FADEOUT:
					projectile.velocity *= 0.95f;
					projectile.alpha += 10;
					if (projectile.alpha >= 255)
						projectile.Kill();
					break;
			}
			++AiTimer;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AiState);
			writer.Write(AiTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AiState = reader.ReadSingle();
			AiTimer = reader.ReadSingle();
		}

		public override bool CanDamage() => !VisualOnly;
	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Projectiles;
using SpiritMod;
using SpiritMod.Particles;
using System.IO;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaStarfire : ModProjectile, ITrailProjectile, IDrawAdditive
    {
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
        }

		public Vector2 InitialVelocity = Vector2.Zero;
		public Vector2 TargetVelocity = Vector2.Zero;

		private const int VelocityLerpTime = 30;

		public float Amplitude = MathHelper.Pi / 16;
		private const float Period = 80;

		private ref float AiTimer => ref projectile.ai[0];

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;

			if(projectile.alpha > 0)
				projectile.alpha = Math.Max(projectile.alpha - 10, 0);

			if (++AiTimer <= VelocityLerpTime) //lerping to desired vel
				projectile.velocity = Vector2.Lerp(InitialVelocity, TargetVelocity, (float)Math.Pow(AiTimer / VelocityLerpTime, 4));

			else //behavior afterwards
				projectile.velocity = TargetVelocity.RotatedBy((float)Math.Sin(MathHelper.TwoPi * (AiTimer - VelocityLerpTime) / Period) * Amplitude);

			if (!Main.dedServ)
			{
				if (Main.rand.NextBool(15))
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f),
						Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime), Color.White, 0.5f), SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.1f, 0.15f), 25));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(TargetVelocity);
			writer.WriteVector2(InitialVelocity);
			writer.Write(Amplitude);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			TargetVelocity = reader.ReadVector2();
			InitialVelocity = reader.ReadVector2();
			Amplitude = reader.ReadSingle();
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime, 1, 0.66f), new RoundCap(), new ArrowGlowPosition(), 100f * projectile.scale, 240f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.02f, 1f, 1f));
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 40 * projectile.scale, 150 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 40 * projectile.scale, 150 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			//star
			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White * projectile.Opacity,
				TriangleHeight = 12 * projectile.scale,
				TriangleWidth = 4 * projectile.scale,
				Position = projectile.Center - Main.screenPosition,
				Rotation = projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);

			//flame trail
			Texture2D extraTex = Main.extraTexture[55];
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			int frame = (int)((Main.GlobalTime * 8) % 4);
			Rectangle drawFrame = new Rectangle(0, frame * extraTex.Height / 4, extraTex.Width, extraTex.Height / 4);
			float scale = projectile.scale * 0.55f;
			Color color = SpiritMod.StarjinxColor(Main.GlobalTime) * projectile.Opacity * 0.5f;
			for (int j = 0; j < ProjectileID.Sets.TrailCacheLength[projectile.type]; j++)
			{
				Vector2 drawPos = projectile.oldPos[j] + projectile.Size / 2;
				float Opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - j) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				Opacity = (Opacity / 2) + 0.5f;
				float trailScale = scale * Opacity;
				sB.Draw(extraTex, drawPos - Main.screenPosition, drawFrame, color * Opacity, projectile.oldRot[j], drawFrame.Size() / 2, trailScale, SpriteEffects.None, 0f);
				sB.Draw(bloom, drawPos - Main.screenPosition, null, color * Opacity * 1.2f, projectile.oldRot[j], bloom.Size() / 2, trailScale * 0.8f, SpriteEffects.None, 0f);
			}

			sB.Draw(extraTex, projectile.Center - Main.screenPosition, drawFrame, color, projectile.rotation, drawFrame.Size() / 2, scale, SpriteEffects.None, 0f);
			sB.Draw(bloom, projectile.Center - Main.screenPosition, null, color * 1.2f, 0, bloom.Size() / 2, scale * 0.8f, SpriteEffects.None, 0f);

			//blur
			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 8f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.3f, 2.5f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.3f, 5f) * Timer;
			Color blurcolor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime), Color.White, 0.2f) * 0.8f * projectile.Opacity;
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffects();

		private void HitEffects()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < 4; i++)
				{
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.8f), Color.White,
						SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.2f, 0.3f), 25));
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!Main.dedServ)
			{
				Vector2 velnormal = Vector2.Normalize(projectile.velocity);
				velnormal *= 4;

				for (int i = 0; i < 2; i++) //weak burst of particles in direction of movement
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));

				for (int i = 0; i < 3; i++) //wide burst of slower moving particles in opposite direction
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(1f, 1.5f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));


				for (int i = 0; i < 2; i++) //narrow burst of faster, bigger particles
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2.5f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.15f) * 0.98f;
							p.Velocity.Y += 0.25f;
						}));

				for (int i = 0; i < 4; i++)
				{
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, oldVelocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.6f), Color.White,
						SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.2f, 0.4f), 25));
				}

				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.35f).WithPitchVariance(0.3f), projectile.Center);
			}
			return true;
		}
    }
}
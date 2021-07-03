using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireProj : ModProjectile, ITrailProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private const int MaxTimeLeft = 180;
        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
            projectile.scale = Main.rand.NextFloat(1f, 1.5f);
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.magic = true;
			projectile.timeLeft = MaxTimeLeft;
			projectile.alpha = 255;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime, 1, 0.2f), new RoundCap(), new ArrowGlowPosition(), 40 * projectile.scale, 300 * projectile.scale);
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 20 * projectile.scale, 300 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
			tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 20 * projectile.scale, 300 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
		}

		public override void Kill(int timeLeft)
        {
			if (Main.dedServ)
				return;

			for(int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.oldVelocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.2f, 0.4f), Color.White, 
					SpiritMod.StarjinxColor(Main.GlobalTime - 1), Main.rand.NextFloat(0.2f, 0.4f), 25));
			}

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit"), projectile.Center);
        }

		private Vector2 OrigVel;
		private ref float AiState => ref projectile.ai[0];
		private const float JustSpawned = 0;
		private const float CosWave = 1;
		private const float Circling = 2;
		private const float Homing = 3;

		private ref float Direction => ref projectile.ai[1];

		private float Timer => MaxTimeLeft - projectile.timeLeft;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			switch (AiState)
			{
				case JustSpawned:
					Direction = Main.rand.NextBool() ? -1 : 1;
					OrigVel = projectile.velocity;
					AiState = Main.rand.NextBool() ? CosWave : Circling;
					break;
				case CosWave:
					projectile.velocity = OrigVel.RotatedBy(Math.Cos(Timer / 60 * MathHelper.TwoPi) * Direction * MathHelper.Pi / 8);
					break;
				case Circling:
					projectile.velocity = (OrigVel.RotatedBy(MathHelper.ToRadians((Timer + 10) * Direction * 5)) * 0.5f) + (OrigVel * 0.33f);
					break;
			}

			if (projectile.frameCounter++ % 5 == 0)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
			}

			if(Main.rand.NextBool(8) && !Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.4f), Color.White * 0.5f,
					SpiritMod.StarjinxColor(Main.GlobalTime - 1) * 0.5f, Main.rand.NextFloat(0.1f, 0.2f), 25));
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			projectile.QuickDrawTrail(spriteBatch, 0.8f, drawColor: Color.White);
			projectile.QuickDraw(spriteBatch, drawColor: Color.White);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.4f, 1.4f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.4f, 2.8f) * Timer;
			Color blurcolor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime - 1), Color.White, 0.33f) * 0.6f * projectile.Opacity;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
			return false;
		}
	}
}
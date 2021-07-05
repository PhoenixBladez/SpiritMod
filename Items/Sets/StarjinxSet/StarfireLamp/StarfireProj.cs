using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System.IO;

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

		private const int MaxTimeLeft = 220;
        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
            projectile.scale = Main.rand.NextFloat(0.7f, 1.2f);
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

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
        }

		private Vector2 OrigVel;
		private ref float AiState => ref projectile.ai[0];
		private const float JustSpawned = 0;
		private const float CosWave = 1;
		private const float Circling = 2;
		private const float HomingAim = 3;
		private const float HomingAccelerate = 4;

		private ref float Direction => ref projectile.ai[1];

		private ref float Timer => ref projectile.localAI[0];

		private NPC Target;

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, SpiritMod.StarjinxColor(Main.GlobalTime - 1).ToVector3() / 3);
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			projectile.tileCollide = Timer > 20;
			void TargetCheck()
			{
				int maxDist = 1000;
				foreach(NPC npc in Main.npc.Where(x => x.Distance(projectile.Center) < maxDist && x.active && x.CanBeChasedBy(this) && x != null)){
					StarfireLampPlayer player = Main.player[projectile.owner].GetModPlayer<StarfireLampPlayer>();
					if (player.LampTargetNPC == npc && npc.active && npc != null && npc.CanBeChasedBy(this))
					{
						AiState = HomingAim;
						Target = npc;
						projectile.netUpdate = true;
						Timer = 0;
					}
				}
			}
			switch (AiState)
			{
				case JustSpawned:
					Direction = Main.rand.NextBool() ? -1 : 1;
					OrigVel = projectile.velocity;
					AiState = Main.rand.NextBool(3) ? Circling : CosWave;
					break;
				case CosWave:
					++Timer;
					projectile.velocity = OrigVel.RotatedBy(Math.Cos(Timer / 60 * MathHelper.TwoPi) * Direction * MathHelper.Pi / 8);
					if(Timer > 10)
						TargetCheck();
					break;
				case Circling:
					++Timer;
					projectile.velocity = (OrigVel.RotatedBy(MathHelper.ToRadians(Timer * Direction * 7) + MathHelper.PiOver2 * Direction) * 0.5f) + (OrigVel * 0.33f);
					if(Timer > 10)
						TargetCheck();
					break;
				case HomingAim:
					++Timer;
					if (Timer > 8 || Target == null || !Target.active || !Target.CanBeChasedBy(this))
					{
						AiState = HomingAccelerate;
						break;
					}
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 3, 0.2f);
					break;
				case HomingAccelerate:
					if (projectile.velocity.Length() < 24)
						projectile.velocity *= 1.05f;

					if (Target != null && Target.active && Target.CanBeChasedBy(this))
						projectile.velocity = projectile.velocity.Length() * 
							Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.1f));
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

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Target.whoAmI);
			writer.Write(Timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Target = Main.npc[reader.ReadInt32()];
			Timer = reader.ReadSingle();
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusConstellationArrow : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Extra_89";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Arrow");

		private const int MaxTimeLeft = 600;
		private const int TileIgnoreTime = 25;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.ranged = true;
			projectile.timeLeft = MaxTimeLeft;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.arrow = true;
		}

		public override bool CanDamage() => projectile.penetrate > 1;

		public Vector2 TargetPos { get; set; }

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.White * 0.2f), new RoundCap(), new ArrowGlowPosition(), 24, 150);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.White), new TriangleCap(), new DefaultTrailPosition(), 12, 150);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.White, Color.Transparent), new TriangleCap(), new DefaultTrailPosition(), 5, 150);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(TargetPos);
			writer.Write(projectile.tileCollide);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			TargetPos = reader.ReadVector2();
			projectile.tileCollide = reader.ReadBoolean();
		}

		public override void AI()
		{
			if(projectile.timeLeft == MaxTimeLeft - TileIgnoreTime)
			{
				projectile.tileCollide = true;
				projectile.netUpdate = true;
			}
			if (projectile.ai[1] == 0)
			{
				float homestrength = MathHelper.Clamp(1 - projectile.Distance(TargetPos) / 500, 0.05f, 0.2f);
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(TargetPos) * 20, homestrength);
				if (projectile.Distance(TargetPos) < 40)
				{
					projectile.ai[1]++;
					projectile.netUpdate = true;
				}
			}
			else if (projectile.velocity.Length() < 25 && projectile.penetrate == projectile.maxPenetrate)
				projectile.velocity *= 1.02f;

			if(projectile.penetrate < projectile.maxPenetrate)
			{
				projectile.velocity *= 0.8f;
				projectile.alpha += 25;
				if (projectile.alpha >= 255)
					projectile.Kill();
			}

			if (Main.rand.NextBool(8) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f), Color.White, Main.rand.NextFloat(0.08f, 0.12f), 25));
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => OnHitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => OnHitEffects();

		private void OnHitEffects()
		{
			if (Main.dedServ)
				return;

			for(int i = -1; i <= 1; i += 2)
			{
				for(int j = 0; j < 2; j++)
				{
					ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center, 
						projectile.velocity.RotatedBy(MathHelper.PiOver2 * i).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.1f, 0.2f), 
						Color.White, new Vector2(0.25f, Main.rand.NextFloat(0.6f, 1f)), 10));
				}
			}
			ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Vector2.Zero, Color.White, Main.rand.NextFloat(0.2f, 0.3f), 10));

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);

			for (int i = 0; i < 3; i++)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f), Color.White, Main.rand.NextFloat(0.1f, 0.2f), 20));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.dedServ)
				return true;

			for(int i = 0; i< 4; i++)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, -oldVelocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.25f), Color.White, Main.rand.NextFloat(0.1f, 0.2f), 20));

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.4f, 1f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.4f, 2f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.4f * projectile.Opacity;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
			return false;
		}
	}
}
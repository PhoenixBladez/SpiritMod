using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
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
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.arrow = true;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.penetrate > 1;

		public Vector2 TargetPos { get; set; }

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.White * 0.2f), new RoundCap(), new ArrowGlowPosition(), 24, 150);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.White), new TriangleCap(), new DefaultTrailPosition(), 12, 150);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.White, Color.Transparent), new TriangleCap(), new DefaultTrailPosition(), 5, 150);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(TargetPos);
			writer.Write(Projectile.tileCollide);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			TargetPos = reader.ReadVector2();
			Projectile.tileCollide = reader.ReadBoolean();
		}

		public override void AI()
		{
			if(Projectile.timeLeft == MaxTimeLeft - TileIgnoreTime)
			{
				Projectile.tileCollide = true;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[1] == 0)
			{
				float homestrength = MathHelper.Clamp(1 - Projectile.Distance(TargetPos) / 500, 0.05f, 0.2f);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(TargetPos) * 20, homestrength);
				if (Projectile.Distance(TargetPos) < 40)
				{
					Projectile.ai[1]++;
					Projectile.netUpdate = true;
				}
			}
			else if (Projectile.velocity.Length() < 25 && Projectile.penetrate == Projectile.maxPenetrate)
				Projectile.velocity *= 1.02f;

			if(Projectile.penetrate < Projectile.maxPenetrate)
			{
				Projectile.velocity *= 0.8f;
				Projectile.alpha += 25;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}

			if (Main.rand.NextBool(8) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f), Color.White, Main.rand.NextFloat(0.08f, 0.12f), 25));
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
					ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center, 
						Projectile.velocity.RotatedBy(MathHelper.PiOver2 * i).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.1f, 0.2f), 
						Color.White, new Vector2(0.25f, Main.rand.NextFloat(0.6f, 1f)), 10));
				}
			}
			ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Vector2.Zero, Color.White, Main.rand.NextFloat(0.2f, 0.3f), 10));

			SoundEngine.PlaySound(SoundID.Item12 with { PitchVariance = 0.2f, Volume = 0.33f }, Projectile.Center);

			for (int i = 0; i < 3; i++)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f), Color.White, Main.rand.NextFloat(0.1f, 0.2f), 20));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.dedServ)
				return true;

			for(int i = 0; i< 4; i++)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, -oldVelocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.25f), Color.White, Main.rand.NextFloat(0.1f, 0.2f), 20));

			SoundEngine.PlaySound(SoundID.NPCHit3 with { PitchVariance = 0.2f }, Projectile.Center);
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.4f, 1f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.4f, 2f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.4f * Projectile.Opacity;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
			return false;
		}
	}
}
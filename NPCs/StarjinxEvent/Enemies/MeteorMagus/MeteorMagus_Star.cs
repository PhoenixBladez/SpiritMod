using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus_Star : ModProjectile, ITrailProjectile
	{
		private const float RADIANS_PERTICK = MathHelper.Pi / 30;
		private const int COLLIDING_TIME = 50;
		
		public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 32);
			Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(228, 31, 156), new Color(180, 88, 237)), new RoundCap(), new ArrowGlowPosition(), 100f, 180f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(241, 153, 255) * .25f, new Color(241, 153, 255) * .125f), new RoundCap(), new ArrowGlowPosition(), 42f, 200f, new DefaultShader());
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(241, 153, 255) * .5f, new Color(241, 153, 255) * .25f), new RoundCap(), new ArrowGlowPosition(), 42f, 40f, new DefaultShader());
		}

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];
		private Player Target => Main.player[(int)Projectile.ai[1]];

		private ref float Timer => ref Projectile.localAI[0];

		private Vector2 homeCenter;

		public float Direction { get; set; }
		public float CirclingTime { get; set; }
		public float Offset_Distance { get; set; }
		public float Offset_Rotation { get; set; }


		public override void AI()
		{
			Projectile.alpha = Math.Max(Projectile.alpha - 5, 0);

			if (!Parent.active || !Target.active || Target.dead)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				//Ease rotation speed over time
				float speed = (CirclingTime - Timer) / CirclingTime;
				speed = EaseFunction.EaseQuadOut.Ease(speed);
				speed = Math.Max(speed, 0.15f);

				Projectile.rotation += speed * Direction / 2;
				float dist = Offset_Distance;

				if (++Timer < CirclingTime) //Before colliding, stick to player center
					homeCenter = Target.Center;
				else //Ease distance to center with cubic function(start slow, end fast) while colliding
					dist = Offset_Distance * (EaseFunction.EaseCubicOut.Ease((COLLIDING_TIME - (Timer - CirclingTime)) / COLLIDING_TIME));

				if (Timer == CirclingTime)
					Projectile.netUpdate = true;

				Projectile.Center = (Vector2.UnitX.RotatedBy(Offset_Rotation) * dist) + homeCenter;
				Offset_Rotation += speed * Direction * RADIANS_PERTICK;

				if (Projectile.Distance(homeCenter) < 10)
					Projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), Projectile.Center);

				for (int i = 0; i < 6; i++)
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Main.rand.NextVector2Circular(6, 6), new Color(241, 153, 255),
						new Color(228, 31, 156), Main.rand.NextFloat(0.1f, 0.2f), 25));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Direction);
			writer.Write(Offset_Distance);
			writer.Write(Offset_Rotation);
			writer.Write(CirclingTime);
			writer.WriteVector2(homeCenter);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Direction = reader.ReadSingle();
			Offset_Distance = reader.ReadSingle();
			Offset_Rotation = reader.ReadSingle();
			CirclingTime = reader.ReadSingle();
			homeCenter = reader.ReadVector2();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(spriteBatch, drawColor: Color.White);
			return false;
		}
	}
}
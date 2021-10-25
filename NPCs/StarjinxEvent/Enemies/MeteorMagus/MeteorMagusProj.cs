using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MortarStar : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 32);
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.hide = true;
		}

		internal NPC Parent => Main.npc[(int)projectile.ai[0]];
		internal Player Target => Main.player[(int)projectile.ai[1]];

		internal ref float Timer => ref projectile.localAI[0];

		private readonly float gravity = 0.3f;

		public override bool PreAI()
		{
			if (!Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center + Main.rand.NextVector2Circular(3, 3), projectile.velocity * 0.1f, new Color(241, 153, 255) * projectile.Opacity, 
					new Color(228, 31, 156) * projectile.Opacity, Main.rand.NextFloat(0.12f, 0.18f), 25));

			return true;
		}

		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 10, 0);
			if (projectile.localAI[1] == 0)
				projectile.localAI[1] = Main.rand.NextBool() ? -0.2f : 0.2f;

			projectile.tileCollide = Timer > 100;
			projectile.rotation += projectile.localAI[1];

			if (!Parent.active || !Target.active || Target.dead)
				projectile.Kill();

			if (++Timer == 20)
				projectile.velocity = GetArcVel().RotatedByRandom(MathHelper.Pi / 12);
			else if (Timer > 20)
				projectile.velocity.Y += gravity;
		}

		private Vector2 GetArcVel()
		{
			Vector2 DistanceToTravel = Target.Center - projectile.Center;
			float MaxHeight = MathHelper.Clamp(DistanceToTravel.Y - 100, -400, -100);
			float TravelTime = (float)Math.Sqrt(-2 * MaxHeight / gravity) + (float)Math.Sqrt(2 * Math.Max(DistanceToTravel.Y - MaxHeight, 0) / gravity);
			return new Vector2(MathHelper.Clamp(DistanceToTravel.X / TravelTime, -15, 15), -(float)Math.Sqrt(-2 * gravity * MaxHeight));
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156), new Color(180, 88, 237)), new RoundCap(), new ArrowGlowPosition(), 100f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .25f, new Color(241, 153, 255) * .125f), new RoundCap(), new ArrowGlowPosition(), 42f, 200f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .5f, new Color(241, 153, 255) * .25f), new RoundCap(), new ArrowGlowPosition(), 42f, 40f, new DefaultShader());
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White * projectile.Opacity,
				TriangleHeight = 12 * projectile.scale,
				TriangleWidth = 4 * projectile.scale,
				Position = projectile.Center - Main.screenPosition,
				Rotation = projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);

				for (int i = 0; i < 5; i++)
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, -projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.75f), new Color(241, 153, 255), 
						new Color(228, 31, 156), Main.rand.NextFloat(0.2f, 0.3f), 25));

				for (int i = 0; i < 3; i++)
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.66f), new Color(241, 153, 255), 
						new Color(228, 31, 156), Main.rand.NextFloat(0.2f, 0.3f), 25));
			}
		}
	}

	public class CirclingStar : MortarStar //inheriting from mortar star to cut down on boilerplate, since they share the same visuals
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
		}

		private Vector2 homeCenter;

		public float Direction;
		public float CirclingTime = 60f;
		public Vector2 Offset;


		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 5, 0);

			if (!Parent.active || !Target.active || Target.dead)
			{
				projectile.Kill();
				return;
			}
			else
			{
				float speed = Math.Max((CirclingTime - Timer) / (CirclingTime * 0.75f), 0.25f);
				projectile.rotation += speed * Direction / 5;

				if (++Timer < 60)
					homeCenter = Target.Center;
				else
					Offset *= (float)Math.Pow((CirclingTime / Timer) / 10 + 0.9f, 3);

				if (Timer == 60)
					projectile.netUpdate = true;

				projectile.Center = Offset + homeCenter;
				Offset = Offset.RotatedBy(speed * Direction * MathHelper.Pi / 30);

				if (projectile.Distance(homeCenter) < 10)
					projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);

				for (int i = 0; i < 6; i++)
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Main.rand.NextVector2Circular(6, 6), new Color(241, 153, 255),
						new Color(228, 31, 156), Main.rand.NextFloat(0.1f, 0.2f), 25));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Direction);
			writer.WriteVector2(Offset);
			writer.WriteVector2(homeCenter);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Direction = reader.ReadSingle();
			Offset = reader.ReadVector2();
			homeCenter = reader.ReadVector2();
		}
	}
}
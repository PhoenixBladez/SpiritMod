using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MortarStar : ModProjectile, ITrailProjectile, IBasicPrimDraw
	{
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 32);
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.hide = true;
		}

		internal NPC Parent => Main.npc[(int)projectile.ai[0]];
		internal Player Target => Main.player[(int)projectile.ai[1]];

		internal ref float Timer => ref projectile.localAI[0];

		private readonly float gravity = 0.3f;

		public override bool PreAI()
		{
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(projectile.position, 6, 6, DustID.FireworkFountain_Pink, 0f, 0f, 0, default(Color), .35f);
				Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;

				Main.dust[num].velocity = projectile.velocity;
				Main.dust[num].scale = MathHelper.Clamp(projectile.ai[0], .015f, 1.25f);
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = (float)(100 + projectile.owner);
			}
			return true;
		}

		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			if (projectile.localAI[1] == 0)
				projectile.localAI[1] = Main.rand.NextBool() ? -0.2f : 0.2f;

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
			tM.CreateTrail(projectile, new GradientTrail(new Color(228, 31, 156), new Color(180, 88, 237)), new RoundCap(), new ArrowGlowPosition(), 100f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .25f, new Color(241, 153, 255) * .125f), new RoundCap(), new ArrowGlowPosition(), 42f, 200f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .5f, new Color(241, 153, 255) * .25f), new RoundCap(), new ArrowGlowPosition(), 42f, 40f, new DefaultShader());
		}

		public void DrawPrimShape(BasicEffect effect) => StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 15, Color.White * projectile.Opacity);

		public override void Kill(int timeLeft)
		{
			DustHelper.DrawStar(projectile.Center, 223, pointAmount: 5, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);
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

		public Vector2 Offset;

		private readonly float circlingtime = 60;

		public override void AI()
		{
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			if (!Parent.active || !Target.active || Target.dead)
			{
				projectile.Kill();
				return;
			}
			else
			{
				float speed = Math.Max((circlingtime - Timer) / (circlingtime * 0.75f), 0.25f);
				projectile.rotation += speed * Direction / 5;

				if (++Timer < 60)
					homeCenter = Target.Center;
				else
					Offset *= (float)Math.Pow((circlingtime / Timer) / 10 + 0.9f, 3);

				if (Timer == 60)
					projectile.netUpdate = true;

				projectile.Center = Offset + homeCenter;
				Offset = Offset.RotatedBy(speed * Direction * MathHelper.Pi / 30);

				if (projectile.Distance(homeCenter) < 10)
					projectile.Kill();
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
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Particles;
using SpiritMod.Prim;
using Terraria.ID;
using System.IO;
using System.Linq;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaStarfire : ModProjectile, ITrailProjectile, IDrawAdditive
    {
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfire");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private const int MAXTIMELEFT = 120;
		private readonly Color Yellow = new Color(242, 240, 134);
		private readonly Color Orange = new Color(255, 98, 74);
		private readonly Color Purple = new Color(255, 0, 144);
		private bool Dying = false; //Used for a less abrupt death, stops movement and starts fadeout when set to true by colliding with tiles


		public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = MAXTIMELEFT;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
			projectile.alpha = 0;
			projectile.scale = Main.rand.NextFloat(0.9f, 1.3f);
			projectile.hide = true;
        }

		private float CircleOffset;
		private float CirclingSpeed;
		private float CircleSize;

		public override bool CanDamage() => !Dying;

		public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;

			if (projectile.timeLeft == MAXTIMELEFT)
			{
				CircleOffset = Main.rand.Next(360);
				CirclingSpeed = Main.rand.NextFloat(9, 12) * (Main.rand.NextBool() ? -1 : 1);
				CircleSize = Main.rand.NextFloat(4, 6);

				projectile.netUpdate = true;
			}

			int fadeOutTime = 20;
			if (Dying)
			{
				projectile.velocity = Vector2.Zero;
				if(projectile.timeLeft > fadeOutTime)
					projectile.timeLeft = fadeOutTime;
			}

			if (projectile.timeLeft <= fadeOutTime)
				projectile.alpha = Math.Min(projectile.alpha + (255 / fadeOutTime), 255);

			if (projectile.velocity.Length() > 18)
				projectile.velocity *= 0.97f;

			//Add circular movement to the projectiles
			Vector2 circularVelocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians((projectile.timeLeft + CircleOffset) * CirclingSpeed)) * CircleSize;
			if(!Dying)
				projectile.position += circularVelocity;

			if (!Dying && !Main.dedServ)
			{
				if(Main.rand.NextBool(8))
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Yellow, Orange, Main.rand.NextFloat(0.25f, 0.3f), 30, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
						}));

				if(Main.rand.NextBool(6))
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.15f, 0.2f), 25));
			}
		}

		public void DoTrailCreation(TrailManager tM)
		{
			//tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(242, 240, 134), new Color(255, 88, 35)), new RoundCap(), new DefaultTrailPosition(), 50f * projectile.scale, 250f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.02f, 1f, 1f));
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			//set the parameters for the shader
			Effect effect = mod.GetEffect("Effects/FlameTrail");
			effect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_3"));
			effect.Parameters["uTexture2"].SetValue(mod.GetTexture("Textures/Trails/Trail_4"));
			effect.Parameters["Progress"].SetValue(Main.GlobalTime * -0.5f);
			effect.Parameters["xMod"].SetValue(1.5f);
			effect.Parameters["StartColor"].SetValue(Yellow.ToVector4());
			effect.Parameters["MidColor"].SetValue(Orange.ToVector4());
			effect.Parameters["EndColor"].SetValue(Purple.ToVector4());

			//draw the strip with the given array

			///Just using the oldpos array with no changes bugs out due to values defaulting to (0, 0) in the world until set, setting all values to projectile.position makes a weird rectangle shape as
			///the projectile spawns in. This solution isn't ideal, as it may bug out when the projectile actually passes through (0, 0), but it works well enough for now
			Vector2[] trimmedOldPos = projectile.oldPos.Where(x => x != Vector2.Zero).ToArray();
			Vector2[] posarray = new Vector2[trimmedOldPos.Length];
			trimmedOldPos.CopyTo(posarray, 0);
			posarray.IterateArray(delegate (ref Vector2 vec, int index, float progress) { vec += projectile.Size / 2; });
			var strip = new PrimitiveStrip
			{
				Color = Color.White * projectile.Opacity,
				Width = 18 * projectile.scale,
				PositionArray = posarray,
				TaperingType = StripTaperType.None,
				WidthDelegate = delegate (float progress) { return ((float)Math.Sin((Main.GlobalTime - progress) * MathHelper.TwoPi * 1.5f) * 0.33f + 1.33f) * (float)Math.Pow(1 - progress, 0.8f); }
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			var circle = new CirclePrimitive
			{
				Color = Color.White * projectile.Opacity,
				Radius = 22f * projectile.scale * (float)(Math.Sin((Main.GlobalTime) * MathHelper.TwoPi * 1.5f) * 0.33f + 1.33f),
				Position = projectile.Center - Main.screenPosition,
				MaxRadians = MathHelper.TwoPi,
				Rotation = projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(circle, effect);

			//Blur lines
			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 8f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.25f, 2f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.25f, 4f * projectile.Opacity) * Timer;
			Color blurcolor = Color.Lerp(Yellow, Color.White, 0.33f) * projectile.Opacity;
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffects();

		private void HitEffects()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < 6; i++)
				{
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, 
						Main.rand.NextVector2Unit() * Main.rand.NextFloat(5) + projectile.velocity/7,
						Yellow, Orange, Main.rand.NextFloat(0.4f, 0.6f), 25, delegate (Particle p)
						{
							p.Velocity.X *= 0.95f;
							p.Velocity.Y += 0.1f;
						}));
				}

				for(int i = 0; i < 4; i++)
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.2f, 0.3f), 25));

				for (int i = 0; i < 3; i++)
					ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center, Main.rand.NextVector2Unit(), Orange * 0.75f, new Vector2(0.5f, Main.rand.NextFloat(1f, 1.4f)), 15));
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!Dying)
			{
				if (!Main.dedServ)
				{
					Vector2 velnormal = Vector2.Normalize(projectile.velocity);
					velnormal *= 4;

					for (int i = 0; i < 2; i++) //weak burst of particles in direction of movement
						ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2f),
							Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
							{
								p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
							}));

					for (int i = 0; i < 3; i++) //wide burst of slower moving particles in opposite direction
						ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(1f, 1.5f),
							Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
							{
								p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
							}));


					for (int i = 0; i < 2; i++) //narrow burst of faster, bigger particles
						ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2.5f),
							Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
							{
								p.Velocity = p.Velocity.RotatedByRandom(0.15f) * 0.98f;
								p.Velocity.Y += 0.25f;
							}));

					for (int i = 0; i < 4; i++)
						ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3),
							Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.2f, 0.3f), 25));

					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.35f).WithPitchVariance(0.3f), projectile.Center);
				}

				//Make projectile stop moving and begin fadeout
				projectile.velocity = Vector2.Zero;
				Dying = true;
				projectile.netUpdate = true;
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Dying);
			writer.Write(CircleOffset);
			writer.Write(CirclingSpeed);
			writer.Write(CircleSize);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Dying = reader.ReadBoolean();
			CircleOffset = reader.ReadSingle();
			CirclingSpeed = reader.ReadSingle();
			CircleSize = reader.ReadSingle();
		}
	}
}
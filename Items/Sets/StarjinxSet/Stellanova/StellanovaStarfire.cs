using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Particles;
using SpiritMod.Prim;
using Terraria.ID;
using System.IO;
using System.Linq;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Mechanics.Trails.CustomTrails;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaStarfire : ModProjectile, ITrailProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starfire");

		private const int MAXTIMELEFT = 120;
		public const float MAX_SPEED = 40f;
		private const float MED_SPEED = 18f;
		private const float MIN_SPEED = 5f;

		public static Color Yellow = new Color(242, 240, 134);
		public static Color Orange = new Color(255, 98, 74);
		public static Color Purple = new Color(255, 0, 144);


		public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = MAXTIMELEFT;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
			Projectile.alpha = 0;
			Projectile.scale = Main.rand.NextFloat(0.85f, 1.1f);
			Projectile.hide = true;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateCustomTrail(new FlameTrail(Projectile, Yellow, Orange, Purple, 28 * Projectile.scale, 14));

		private float CircleOffset;
		private float CirclingSpeed;
		private float CircleSize;

		public override void AI()
        {
			if (Projectile.timeLeft == MAXTIMELEFT)
			{
				CircleOffset = Main.rand.Next(360);
				CirclingSpeed = Main.rand.NextFloat(4, 5) * (Main.rand.NextBool() ? -1 : 1);
				CircleSize = Main.rand.NextFloat(8, 9);

				Projectile.netUpdate = true;
			}

			int fadeOutTime = 20;

			if (Projectile.timeLeft <= fadeOutTime)
				Projectile.alpha = Math.Min(Projectile.alpha + (255 / fadeOutTime), 255);

			//If projectile is above the medium speed, slow down more harshly, and slow down constantly while above the minimum speed
			if (Projectile.velocity.Length() > MED_SPEED)
				Projectile.velocity *= 0.98f;

			if (Projectile.velocity.Length() > MIN_SPEED)
				Projectile.velocity *= 0.985f;

			//Add circular movement to the projectiles, based on how slowly they're moving
			Vector2 circularVelocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians((Projectile.timeLeft + CircleOffset) * CirclingSpeed)) * CircleSize;
			Projectile.position += circularVelocity * (float)Math.Pow(1 - (Projectile.velocity.Length() / MAX_SPEED), 2f);

			Projectile.rotation += 0.15f * (Math.Sign(Projectile.velocity.X) > 0 ? 1 : -1);

			if (!Main.dedServ)
			{
				if(Main.rand.NextBool(8))
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(0.75f),
						Yellow, Orange, Main.rand.NextFloat(0.25f, 0.3f), 30, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
						}));

				if(Main.rand.NextBool(6))
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(0.75f),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.15f, 0.2f), 25));
			}
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			float blurLength = 180 * Projectile.scale;
			float blurWidth = 8 * Projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 12) % 1) * 0.1f) + 1f;
			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			IPrimitiveShape[] blurLines = new IPrimitiveShape[]
			{
				//Horizontal
				new SquarePrimitive()
				{
					Position = Projectile.Center - Main.screenPosition,
					Height = blurWidth * flickerStrength,
					Length = blurLength * flickerStrength,
					Rotation = 0,
					Color = Color.White * flickerStrength * Projectile.Opacity
				},

				//Vertical, lower length
				new SquarePrimitive()
				{
					Position = Projectile.Center - Main.screenPosition,
					Height = blurWidth * flickerStrength,
					Length = blurLength * flickerStrength * 0.75f,
					Rotation = MathHelper.PiOver2,
					Color = Color.White * flickerStrength * Projectile.Opacity
				},
			};

			PrimitiveRenderer.DrawPrimitiveShapeBatched(blurLines, blurEffect);

			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			sB.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, bloom.Size() / 2, 0.25f * Projectile.scale, SpriteEffects.None, 0);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffects();

		private void HitEffects()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < 6; i++)
				{
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, 
						Main.rand.NextVector2Unit() * Main.rand.NextFloat(5) + Projectile.velocity/7,
						Yellow, Orange, Main.rand.NextFloat(0.4f, 0.6f), 25, delegate (Particle p)
						{
							p.Velocity.X *= 0.95f;
							p.Velocity.Y += 0.1f;
						}));
				}

				for(int i = 0; i < 4; i++)
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.2f, 0.3f), 25));

				for (int i = 0; i < 3; i++)
					ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center, Main.rand.NextVector2Unit(), Orange * 0.75f, new Vector2(0.5f, Main.rand.NextFloat(1f, 1.4f)), 15));
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!Main.dedServ)
			{
				Vector2 velnormal = Vector2.Normalize(Projectile.velocity);
				velnormal *= 4;

				for (int i = 0; i < 2; i++) //weak burst of particles in direction of movement
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2f),
						Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));

				for (int i = 0; i < 3; i++) //wide burst of slower moving particles in opposite direction
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, -velnormal.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(1f, 1.5f),
						Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));


				for (int i = 0; i < 2; i++) //narrow burst of faster, bigger particles
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, -velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2.5f),
						Yellow, Orange, Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.15f) * 0.98f;
							p.Velocity.Y += 0.25f;
						}));

				for (int i = 0; i < 4; i++)
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.2f, 0.3f), 25));

				SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.35f, PitchVariance = 0.3f }, Projectile.Center);
			}

			//Make projectile stop moving and begin fadeout
			Projectile.velocity = Vector2.Zero;
			Projectile.netUpdate = true;

			return true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(CircleOffset);
			writer.Write(CirclingSpeed);
			writer.Write(CircleSize);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			CircleOffset = reader.ReadSingle();
			CirclingSpeed = reader.ReadSingle();
			CircleSize = reader.ReadSingle();
		}
	}
}
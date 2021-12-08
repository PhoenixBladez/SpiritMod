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
using SpiritMod.Mechanics.Trails;
using SpiritMod.Mechanics.Trails.CustomTrails;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaStarfire : ModProjectile, ITrailProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starfire");

		private const int MAXTIMELEFT = 120;
		private readonly Color Yellow = new Color(242, 240, 134);
		private readonly Color Orange = new Color(255, 98, 74);
		private readonly Color Purple = new Color(255, 0, 144);


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
			projectile.scale = Main.rand.NextFloat(0.85f, 1.1f);
			projectile.hide = true;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateCustomTrail(new FlameTrail(projectile, Yellow, Orange, Purple, 30 * projectile.scale, 12));

		private float CircleOffset;
		private float CirclingSpeed;
		private float CircleSize;

		public override void AI()
        {

			if (projectile.timeLeft == MAXTIMELEFT)
			{
				CircleOffset = Main.rand.Next(360);
				CirclingSpeed = Main.rand.NextFloat(6, 8) * (Main.rand.NextBool() ? -1 : 1);
				CircleSize = Main.rand.NextFloat(3, 4);

				projectile.netUpdate = true;
			}

			int fadeOutTime = 20;

			if (projectile.timeLeft <= fadeOutTime)
				projectile.alpha = Math.Min(projectile.alpha + (255 / fadeOutTime), 255);

			if (projectile.velocity.Length() > 20)
				projectile.velocity *= 0.97f;

			//Add circular movement to the projectiles
			Vector2 circularVelocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians((projectile.timeLeft + CircleOffset) * CirclingSpeed)) * CircleSize;
			projectile.position += circularVelocity;

			projectile.rotation += 0.15f * (Math.Sign(projectile.velocity.X) > 0 ? 1 : -1);

			if (!Main.dedServ)
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

		public void AdditiveCall(SpriteBatch sB)
		{
			float blurLength = 180 * projectile.scale;
			float blurWidth = 8 * projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTime * 12) % 1) * 0.1f) + 1f;
			Effect blurEffect = mod.GetEffect("Effects/BlurLine");

			IPrimitiveShape[] blurLines = new IPrimitiveShape[]
			{
				//Horizontal
				new SquarePrimitive()
				{
					Position = projectile.Center - Main.screenPosition,
					Height = blurWidth * flickerStrength,
					Length = blurLength * flickerStrength,
					Rotation = 0,
					Color = Color.White * flickerStrength * projectile.Opacity
				},

				//Vertical, lower length
				new SquarePrimitive()
				{
					Position = projectile.Center - Main.screenPosition,
					Height = blurWidth * flickerStrength,
					Length = blurLength * flickerStrength * 0.75f,
					Rotation = MathHelper.PiOver2,
					Color = Color.White * flickerStrength * projectile.Opacity
				},
			};

			PrimitiveRenderer.DrawPrimitiveShapeBatched(blurLines, blurEffect);

			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			sB.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, 0, bloom.Size() / 2, 0.25f * projectile.scale, SpriteEffects.None, 0);
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
			projectile.netUpdate = true;

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
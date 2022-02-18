using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System.IO;
using SpiritMod.Prim;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Mechanics.Trails.CustomTrails;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
	public class StarfireProj : ModProjectile, IDrawAdditive, ITrailProjectile
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.Homing[projectile.type] = true;
		}

		private readonly Color Yellow = new Color(242, 240, 134);
		private readonly Color Orange = new Color(255, 98, 74);
		private readonly Color Purple = new Color(255, 0, 144);
		public void DoTrailCreation(TrailManager tM) => tM.CreateCustomTrail(new FlameTrail(projectile, Yellow, Orange, Purple, 26 * projectile.scale, 9));

		private const int MaxTimeLeft = 240;
        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
            projectile.scale = Main.rand.NextFloat(0.8f, 1.3f);
            projectile.friendly = true;
            projectile.magic = true;
			projectile.timeLeft = MaxTimeLeft;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
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
			projectile.tileCollide = Timer > 15;
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
					projectile.velocity = OrigVel.RotatedBy(Math.Cos((Timer / 45) * MathHelper.TwoPi) * Direction * MathHelper.Pi / 8);
					if(Timer > 10)
						TargetCheck();
					break;
				case Circling:
					++Timer;
					projectile.velocity = (OrigVel.RotatedBy(MathHelper.ToRadians(Timer * Direction * 9) + MathHelper.PiOver2 * Direction) * 0.5f) + (OrigVel * 0.33f);
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
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 2, 0.22f);
					break;
				case HomingAccelerate:
					if (projectile.velocity.Length() < 24)
						projectile.velocity *= 1.05f;

					if (Target != null && Target.active && Target.CanBeChasedBy(this))
						projectile.velocity = projectile.velocity.Length() * 
							Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.1f));
					break;
			}

			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			projectile.alpha = Math.Max(projectile.alpha - 15, 0);

			if (projectile.frameCounter++ % 5 == 0)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
			}

			if(!Main.dedServ)
			{
				if (Main.rand.NextBool(12))
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Yellow, Orange, Main.rand.NextFloat(0.25f, 0.3f), 25, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
						}));

				if (Main.rand.NextBool(8))
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.15f, 0.2f), 20));
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => OnCollision();

		public override void OnHitPlayer(Player target, int damage, bool crit) => OnCollision();

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			OnCollision();
			return true;
		}

		private void OnCollision()
		{
			projectile.rotation = projectile.oldVelocity.ToRotation() - MathHelper.PiOver2;
			projectile.velocity = Vector2.Zero;
			projectile.netUpdate = true;

			if (Main.dedServ)
				return;

			for (int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.oldVelocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.2f, 0.4f), Color.White,
					SpiritMod.StarjinxColor(Main.GlobalTime - 1), Main.rand.NextFloat(0.2f, 0.4f), 25));
			}

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Target == null ? -1 : Target.whoAmI);
			writer.Write(Timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			int whoami = reader.ReadInt32();
			Target = whoami == -1 ? null : Main.npc[whoami];
			Timer = reader.ReadSingle();
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float blurLength = 100 * projectile.scale;
			float blurWidth = 5 * projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTime * 10) % 1) * 0.1f) + 1f;
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
					Length = blurLength * flickerStrength * 0.5f,
					Rotation = MathHelper.PiOver2,
					Color = Color.White * flickerStrength * projectile.Opacity
				},
			};

			PrimitiveRenderer.DrawPrimitiveShapeBatched(blurLines, blurEffect);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}
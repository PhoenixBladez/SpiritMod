using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.Audio;
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
		public override string Texture => "Terraria/Images/Projectile_1";

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		private readonly Color Yellow = new Color(242, 240, 134);
		private readonly Color Orange = new Color(255, 98, 74);
		private readonly Color Purple = new Color(255, 0, 144);
		public void DoTrailCreation(TrailManager tM) => tM.CreateCustomTrail(new FlameTrail(Projectile, Yellow, Orange, Purple, 26 * Projectile.scale, 9));

		private const int MaxTimeLeft = 240;
        public override void SetDefaults()
        {
			Projectile.Size = new Vector2(20, 20);
            Projectile.scale = Main.rand.NextFloat(0.8f, 1.3f);
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		private Vector2 OrigVel;
		private ref float AiState => ref Projectile.ai[0];
		private const float JustSpawned = 0;
		private const float CosWave = 1;
		private const float Circling = 2;
		private const float HomingAim = 3;
		private const float HomingAccelerate = 4;

		private ref float Direction => ref Projectile.ai[1];

		private ref float Timer => ref Projectile.localAI[0];

		private NPC Target;
		
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly - 1).ToVector3() / 3);
			Projectile.tileCollide = Timer > 15;
			void TargetCheck()
			{
				int maxDist = 1000;
				foreach(NPC npc in Main.npc.Where(x => x.Distance(Projectile.Center) < maxDist && x.active && x.CanBeChasedBy(this) && x != null)){
					//StarfireLampPlayer player = Main.player[Projectile.owner].GetModPlayer<StarfireLampPlayer>(); //NEEDSUPDATING
					//if (player.LampTargetNPC == npc && npc.active && npc != null && npc.CanBeChasedBy(this))
					//{
					//	AiState = HomingAim;
					//	Target = npc;
					//	Projectile.netUpdate = true;
					//	Timer = 0;
					//}
				}
			}

			switch (AiState)
			{
				case JustSpawned:
					Direction = Main.rand.NextBool() ? -1 : 1;
					OrigVel = Projectile.velocity;
					AiState = Main.rand.NextBool(3) ? Circling : CosWave;
					break;
				case CosWave:
					++Timer;
					Projectile.velocity = OrigVel.RotatedBy(Math.Cos((Timer / 45) * MathHelper.TwoPi) * Direction * MathHelper.Pi / 8);
					if(Timer > 10)
						TargetCheck();
					break;
				case Circling:
					++Timer;
					Projectile.velocity = (OrigVel.RotatedBy(MathHelper.ToRadians(Timer * Direction * 9) + MathHelper.PiOver2 * Direction) * 0.5f) + (OrigVel * 0.33f);
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
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * 2, 0.22f);
					break;
				case HomingAccelerate:
					if (Projectile.velocity.Length() < 24)
						Projectile.velocity *= 1.05f;

					if (Target != null && Target.active && Target.CanBeChasedBy(this))
						Projectile.velocity = Projectile.velocity.Length() * 
							Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * Projectile.velocity.Length(), 0.1f));
					break;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			Projectile.alpha = Math.Max(Projectile.alpha - 15, 0);

			if (Projectile.frameCounter++ % 5 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
			}

			if(!Main.dedServ)
			{
				if (Main.rand.NextBool(12))
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(0.75f),
						Yellow, Orange, Main.rand.NextFloat(0.25f, 0.3f), 25, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
						}));

				if (Main.rand.NextBool(8))
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(0.75f),
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
			Projectile.rotation = Projectile.oldVelocity.ToRotation() - MathHelper.PiOver2;
			Projectile.velocity = Vector2.Zero;
			Projectile.netUpdate = true;

			if (Main.dedServ)
				return;

			for (int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.oldVelocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.2f, 0.4f), Color.White,
					SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly - 1), Main.rand.NextFloat(0.2f, 0.4f), 25));
			}

			SoundEngine.PlaySound(SoundID.Item12 with { PitchVariance = 0.2f, Volume = 0.3f }, Projectile.Center);
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
			float blurLength = 100 * Projectile.scale;
			float blurWidth = 5 * Projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 10) % 1) * 0.1f) + 1f;
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
					Length = blurLength * flickerStrength * 0.5f,
					Rotation = MathHelper.PiOver2,
					Color = Color.White * flickerStrength * Projectile.Opacity
				},
			};

			PrimitiveRenderer.DrawPrimitiveShapeBatched(blurLines, blurEffect);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
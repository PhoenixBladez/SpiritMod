using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlingHammerSubclass
{
	public class PossessedHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			Tooltip.SetDefault("Throws a returning, vengeful hammer that will seek out enemies to hit additional times");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 100;
			Item.width = 40;
			Item.height = 32;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 8f;
			Item.knockBack = 5f;
			Item.damage = 120;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<PossessedHammerProj>();
		}
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<PossessedHammerProjReturning>()] == 0;
	}

	public class PossessedHammerProj : SlingHammerProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		protected override int height => 46;
		protected override int width => 50;
		protected override int chargeTime => 40;
		protected override float chargeRate => 0.7f;
		protected override int thrownProj => ModContent.ProjectileType<PossessedHammerProjReturning>();
		protected override float damageMult => 1.25f;
		protected override int throwSpeed => 46;

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * 0.75f);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)(damage * 0.75f);
	}

	public class PossessedHammerProjReturning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 44;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 700;
		}

		private ref float AiState => ref Projectile.ai[0];
		private const int STATE_THROWOUT = 0;
		private const int STATE_RETURN = 1;
		private const int STATE_SLOWHOME = 2;
		private const int STATE_FASTHOME = 3;

		private ref float AiTimer => ref Projectile.ai[1];
		private ref float NumHits => ref Projectile.localAI[0];

		private float _glowOpacity = 1;
		private float _rotationSpeed = 0.2f;

		public override void AI()
		{
			AiTimer++;
			Projectile.tileCollide = (AiState != STATE_RETURN);

			if (!Main.dedServ)
			{
				if (Projectile.timeLeft % 9 == 0)
					SoundEngine.PlaySound(SoundID.Item19.WithPitchVariance(0.3f).WithVolume(0.7f), Projectile.Center);

				if(Main.rand.NextBool(3) && _glowOpacity > 0)
				{
					Vector2 vel = (Projectile.velocity / 5).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.8f, 1.2f);
					int maxtime = Main.rand.Next(35, 46);
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center + Main.rand.NextVector2Circular(20, 20), vel,
						new Color(255, 143, 216) * _glowOpacity * 0.7f, new Color(57, 14, 92) * _glowOpacity * 0.7f, Main.rand.NextFloat(0.5f, 0.7f), maxtime, delegate (Particle particle)
						{
							particle.Velocity = vel.RotatedBy(Math.Cos(MathHelper.TwoPi * (particle.TimeActive / (float)maxtime)) * (MathHelper.Pi / 5));
							particle.Velocity *= 0.97f;
						}));
				}
			}

			Player Owner = Main.player[Projectile.owner];
			NPC target = null;
			Projectile.rotation += Projectile.velocity.X > 0 ? _rotationSpeed : -_rotationSpeed;

			if (NumHits >= 3)
				SetAIState(STATE_RETURN);

			switch (AiState) 
			{
				case STATE_THROWOUT:
					AiTimer++;
					Projectile.rotation += Projectile.velocity.X > 0 ? 0.2f : -0.2f;
					Projectile.velocity *= 0.96f;
					target = Projectiles.ProjectileExtras.FindNearestNPC(Projectile.Center, 200, false);
					if (target != null)
						Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Projectile.velocity.Length(), 0.15f));

					if (AiTimer > 40)
						SetAIState(STATE_RETURN);
					break;
				case STATE_RETURN:
					AiTimer++;
					_glowOpacity = Math.Max(_glowOpacity - 0.1f, 0);
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Owner.Center) * 20, 0.15f);
					if (AiTimer > 60)
						Projectile.extraUpdates = 1;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.2f, 0.1f);
					if (Projectile.Hitbox.Intersects(Owner.Hitbox))
						Projectile.Kill();
					break;
				case STATE_SLOWHOME:
					AiTimer++;
					if(Projectile.velocity.Length() > 4)
						Projectile.velocity *= 0.95f;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.4f, 0.2f);
					target = Projectiles.ProjectileExtras.FindNearestNPC(Projectile.Center, 600, false);
					if (target != null)
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center), 0.08f);

					if (AiTimer > 38)
						SetAIState(STATE_FASTHOME);
					break;
				case STATE_FASTHOME:
					AiTimer++;
					if (Projectile.velocity.Length() < 24)
						Projectile.velocity *= 1.1f;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.1f, 0.2f);
					target = Projectiles.ProjectileExtras.FindNearestNPC(Projectile.Center, 600, false);
					if (target != null)
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 24, 0.12f);
					else
						SetAIState(STATE_RETURN);

					if(AiTimer > 90) // just in case
					{
						NumHits++;
						SetAIState(STATE_SLOWHOME);
					}
					break;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.damage = (int)(Projectile.damage * 0.9f);
			if (AiState == STATE_THROWOUT || AiState == STATE_FASTHOME)
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
					ParticleHandler.SpawnParticle(new PulseCircle(Projectile.Center, new Color(217, 0, 255) * 0.25f, 150, 12) {RingColor = new Color(255, 94, 239) * 0.5f });
					ParticleHandler.SpawnParticle(new PulseCircle(Projectile.Center, new Color(217, 0, 255) * 0.2f, 200, 12) { RingColor = new Color(255, 94, 239) * 0.4f });
					for (int i = 0; i < 6; i++)
						ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, (Projectile.velocity / 4) - (Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 3) * Main.rand.NextFloat(6)),
							new Color(255, 143, 216), new Color(57, 14, 92), Main.rand.NextFloat(0.5f, 0.7f), 40, delegate (Particle particle)
							{
								if (particle.Velocity.Y < 16)
									particle.Velocity.Y += 0.12f;
							}));

					for (int i = 0; i < 7; i++)
					{
						float velLength = Main.rand.NextFloat(3, 5);
						Vector2 vel = Main.rand.NextVector2Unit() * velLength;
						ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center, vel, new Color(255, 143, 216), new Vector2(0.25f, 2f * (velLength/3f)), 16));
					}
				}

				NumHits++;
				SetAIState(STATE_SLOWHOME);
				Projectile.velocity = -Projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * 1.5f;
				player.GetModPlayer<MyPlayer>().Shake += 8;
			}
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => AiState != STATE_SLOWHOME;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Bounce(oldVelocity, 0.7f);
			if (AiState == STATE_SLOWHOME)
				return false;

			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += 8;
			SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
			if(AiState == STATE_THROWOUT)
				SetAIState(STATE_RETURN);
			else if(AiState == STATE_FASTHOME)
			{
				NumHits++;
				SetAIState(STATE_SLOWHOME);
			}
			return false;
		}

		private void SetAIState(float State)
		{
			AiState = State;
			AiTimer = 0;
			Projectile.netUpdate = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height /= 2;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(spriteBatch, drawColor: Color.Lerp(lightColor, Color.Purple, _glowOpacity));
			Projectile.QuickDrawGlowTrail(spriteBatch, 0.75f * _glowOpacity);
			Projectile.QuickDraw(spriteBatch);
			Projectile.QuickDrawGlow(spriteBatch, Color.White * _glowOpacity);
			return true;
		}
	}
}
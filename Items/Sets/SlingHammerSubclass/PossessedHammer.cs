using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
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
			item.useStyle = 100;
			item.width = 40;
			item.height = 32;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 8f;
			item.knockBack = 5f;
			item.damage = 120;
			item.autoReuse = true;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<PossessedHammerProj>();
		}
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<PossessedHammerProjReturning>()] == 0;
	}

	public class PossessedHammerProj : SlingHammerProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 44;
			projectile.height = 44;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 700;
		}

		private ref float AiState => ref projectile.ai[0];
		private const int STATE_THROWOUT = 0;
		private const int STATE_RETURN = 1;
		private const int STATE_SLOWHOME = 2;
		private const int STATE_FASTHOME = 3;

		private ref float AiTimer => ref projectile.ai[1];
		private ref float NumHits => ref projectile.localAI[0];

		private float _glowOpacity = 1;
		private float _rotationSpeed = 0.2f;

		public override void AI()
		{
			AiTimer++;
			projectile.tileCollide = (AiState != STATE_RETURN);

			if (!Main.dedServ)
			{
				if (projectile.timeLeft % 9 == 0)
					Main.PlaySound(SoundID.Item19.WithPitchVariance(0.3f).WithVolume(0.7f), projectile.Center);

				if(Main.rand.NextBool(3) && _glowOpacity > 0)
				{
					Vector2 vel = (projectile.velocity / 5).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.8f, 1.2f);
					int maxtime = Main.rand.Next(35, 46);
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center + Main.rand.NextVector2Circular(20, 20), vel,
						new Color(255, 143, 216) * _glowOpacity * 0.7f, new Color(57, 14, 92) * _glowOpacity * 0.7f, Main.rand.NextFloat(0.5f, 0.7f), maxtime, delegate (Particle particle)
						{
							particle.Velocity = vel.RotatedBy(Math.Cos(MathHelper.TwoPi * (particle.TimeActive / (float)maxtime)) * (MathHelper.Pi / 5));
							particle.Velocity *= 0.97f;
						}));
				}
			}

			Player Owner = Main.player[projectile.owner];
			NPC target = null;
			projectile.rotation += projectile.velocity.X > 0 ? _rotationSpeed : -_rotationSpeed;

			if (NumHits >= 3)
				SetAIState(STATE_RETURN);

			switch (AiState) 
			{
				case STATE_THROWOUT:
					AiTimer++;
					projectile.rotation += projectile.velocity.X > 0 ? 0.2f : -0.2f;
					projectile.velocity *= 0.96f;
					target = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, 200, false);
					if (target != null)
						projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * projectile.velocity.Length(), 0.15f));

					if (AiTimer > 40)
						SetAIState(STATE_RETURN);
					break;
				case STATE_RETURN:
					AiTimer++;
					_glowOpacity = Math.Max(_glowOpacity - 0.1f, 0);
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Owner.Center) * 20, 0.1f);
					if (AiTimer > 60)
						projectile.extraUpdates = 1;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.2f, 0.1f);
					if (projectile.Hitbox.Intersects(Owner.Hitbox))
						projectile.Kill();
					break;
				case STATE_SLOWHOME:
					AiTimer++;
					if(projectile.velocity.Length() > 4)
						projectile.velocity *= 0.95f;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.4f, 0.2f);
					target = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, 600, false);
					if (target != null)
						projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center), 0.08f);

					if (AiTimer > 38)
						SetAIState(STATE_FASTHOME);
					break;
				case STATE_FASTHOME:
					AiTimer++;
					if (projectile.velocity.Length() < 24)
						projectile.velocity *= 1.1f;

					_rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0.1f, 0.2f);
					target = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, 600, false);
					if (target != null)
						projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 24, 0.12f);
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
			Player player = Main.player[projectile.owner];
			projectile.damage = (int)(projectile.damage * 0.9f);
			if (AiState == STATE_THROWOUT || AiState == STATE_FASTHOME)
			{
				if (!Main.dedServ)
				{
					Main.PlaySound(SoundID.Item88, projectile.Center);
					ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, new Color(217, 0, 255) * 0.25f, 150, 12) {RingColor = new Color(255, 94, 239) * 0.5f });
					ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, new Color(217, 0, 255) * 0.2f, 200, 12) { RingColor = new Color(255, 94, 239) * 0.4f });
					for (int i = 0; i < 6; i++)
						ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, (projectile.velocity / 4) - (Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 3) * Main.rand.NextFloat(6)),
							new Color(255, 143, 216), new Color(57, 14, 92), Main.rand.NextFloat(0.5f, 0.7f), 40, delegate (Particle particle)
							{
								if (particle.Velocity.Y < 16)
									particle.Velocity.Y += 0.12f;
							}));

					for (int i = 0; i < 7; i++)
					{
						float velLength = Main.rand.NextFloat(3, 5);
						Vector2 vel = Main.rand.NextVector2Unit() * velLength;
						ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center, vel, new Color(255, 143, 216), new Vector2(0.25f, 2f * (velLength/3f)), 16));
					}
				}

				NumHits++;
				SetAIState(STATE_SLOWHOME);
				projectile.velocity = -projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * 2;
				player.GetModPlayer<MyPlayer>().Shake += 8;
			}
		}

		public override bool CanDamage() => AiState != STATE_SLOWHOME;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Bounce(oldVelocity, 0.7f);
			if (AiState == STATE_SLOWHOME)
				return false;

			Player player = Main.player[projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += 8;
			Main.PlaySound(SoundID.Item88, projectile.Center);
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
			projectile.netUpdate = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = height /= 2;
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch, drawColor: Color.Lerp(lightColor, Color.Purple, _glowOpacity));
			projectile.QuickDrawGlowTrail(spriteBatch, 0.75f * _glowOpacity);
			projectile.QuickDraw(spriteBatch);
			projectile.QuickDrawGlow(spriteBatch, Color.White * _glowOpacity);
			return true;
		}
	}
}
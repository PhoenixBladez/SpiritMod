using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SpiritMod.Particles;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.LaunchersMisc.Liberty
{
	public class LibertyProjHeld : Projectiles.BaseProj.BaseHeldProj
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Liberty");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(30, 30);
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
		}

		public override bool CanDamage() => false;

		private ref float AiState => ref projectile.ai[0];

		private const int STATE_CHARGEUP = 0;
		private const int STATE_LAUNCH = 1;
		private const int STATE_COOLDOWN = 2;

		private const int CHARGEUPTIME = 40;
		private const int COOLDOWNTIME = 30;
		private ref float AiTimer => ref projectile.ai[1];
		public override void AbstractAI()
		{
			Vector2 shootPos = projectile.Center + projectile.velocity * 40;
			if (!Collision.CanHit(projectile.Center, 0, 0, shootPos, 0, 0))
				shootPos = projectile.Center;

			switch (AiState) 
			{
				case STATE_CHARGEUP:
					if (ChannelKillCheck())
						break;

					if (AiTimer < CHARGEUPTIME * 0.66f && !Main.dedServ && Main.rand.NextBool())
						ParticleHandler.SpawnParticle(new LibertyChargeFire(projectile, new Vector2(Main.rand.NextFloat(30, 40), 0).RotatedByRandom(MathHelper.Pi / 3), new Color(252, 249, 159) * 0.5f, new Color(255, 45, 13) * 0.8f, Main.rand.NextFloat(0.35f, 0.5f), 25));

					if (AiTimer > CHARGEUPTIME)
						SetAIState(STATE_LAUNCH);
					break;
				case STATE_LAUNCH:
					int shoot = 0;
					float speed = 0;
					bool canShoot = true;
					int damage = 0;
					float knockback = 0;
					Owner.PickAmmo(Owner.HeldItem, ref shoot, ref speed, ref canShoot, ref damage, ref knockback); //first pickammo to find stats and actually consume the ammo

					void FixDumbVanillaRocketIDS() => shoot += 134; //required, as for god knows what reason vanilla rockets shoot the wrong projectile(but not modded rockets!!!)
					if (shoot <= 0) //rocket 1
						FixDumbVanillaRocketIDS();
					else 
					{
						Projectile dummy = new Projectile(); //create an instance of a projectile from the shoot id, and check if it's vanilla
						dummy.SetDefaults(shoot);
						if (dummy.modProjectile == null)
							FixDumbVanillaRocketIDS();
					}

					if (!Main.dedServ) //smoke burst before recoil
					{
						for (int i = 0; i < 7; i++) //front smoke particle burst
							ParticleHandler.SpawnParticle(new SmokeParticle(shootPos, projectile.velocity.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(4), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.4f, 0.6f), 30));

						for (int i = 0; i < 4; i++) //back smoke particle burst
							ParticleHandler.SpawnParticle(new SmokeParticle(projectile.Center - (projectile.velocity * 40), -projectile.velocity.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(4), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.3f, 0.5f), 25));
					}

					Projectile.NewProjectile(shootPos, projectile.velocity * (speed + Owner.HeldItem.shootSpeed), shoot, damage + projectile.damage, knockback + projectile.knockBack, Owner.whoAmI);

					projectile.velocity = (projectile.direction > 0) ? projectile.velocity.RotatedBy(-MathHelper.PiOver4) : projectile.velocity.RotatedBy(MathHelper.PiOver4); //recoil effect
					Owner.PickAmmo(Owner.HeldItem, ref shoot, ref speed, ref canShoot, ref damage, ref knockback, true); //second to determine if another shot can be fired
					if (!canShoot) //if no ammo left, stop channelling, so weapon use can end when wanted
						Owner.channel = false;

					if (!Main.dedServ) //fire burst after recoil
					{
						Main.PlaySound(SoundID.Item14.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);

						for (int i = 0; i < 5; i++)
							ParticleHandler.SpawnParticle(new FireParticle(shootPos, projectile.velocity.RotatedByRandom(MathHelper.Pi / 5) * Main.rand.NextFloat(6),
										new Color(246, 255, 0), new Color(232, 37, 2), Main.rand.NextFloat(0.35f, 0.5f), 20, delegate (Particle particle)
										{
											if (particle.Velocity.Y < 16)
												particle.Velocity.Y += 0.12f;
										}));
					}

					SetAIState(STATE_COOLDOWN);
					break;
				case STATE_COOLDOWN:
					if (AiTimer > COOLDOWNTIME)
						SetAIState(STATE_CHARGEUP);

					projectile.velocity = (projectile.direction > 0) ? projectile.velocity.RotatedBy(MathHelper.PiOver4 / COOLDOWNTIME) : projectile.velocity.RotatedBy(-MathHelper.PiOver4 / COOLDOWNTIME); //readjust back to what direction was before shot
					if (!Main.dedServ && Main.rand.NextBool())
						ParticleHandler.SpawnParticle(new SmokeParticle(shootPos, projectile.velocity.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(4), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.2f, 0.25f), 25));
					
					break;
			}
			++AiTimer;
		}

		public override bool AutoAimCursor() => AiState != STATE_COOLDOWN;
		public override Vector2 HoldoutOffset() => new Vector2(-6, 0).RotatedBy(projectile.velocity.ToRotation());

		private void SetAIState(int State)
		{
			AiState = State;
			AiTimer = 0;
			projectile.netUpdate = true;
		}

		public override float CursorLerpSpeed() => 0.2f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Clubs
{
	public abstract class ClubProj : ModProjectile
	{
		public readonly int ChargeTime;
		private readonly int DustType;
		private readonly int Size;
		private readonly int MinKnockback;
		private readonly int MaxKnockback;
		private readonly float Acceleration;
		private readonly float MaxSpeed;

		public int minDamage;
		public int maxDamage;

		public ClubProj(int chargetime, int mindamage, int maxdamage, int dusttype, int size, int minknockback, int maxknockback, float acceleration, float maxspeed)
		{
			ChargeTime = chargetime;
			DustType = dusttype;
			Size = size;
			MinKnockback = minknockback;
			MaxKnockback = maxknockback;
			Acceleration = acceleration;
			MaxSpeed = maxspeed;

			minDamage = mindamage;
			maxDamage = maxdamage;
		}

		public virtual void SafeAI() { }
		public virtual void SafeDraw(SpriteBatch spriteBatch, Color lightColor) { }
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.melee = true;
			projectile.width = projectile.height = 48;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			SafeSetDefaults();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[projectile.owner].Center, projectile.Center) ? true : base.Colliding(projHitbox, targetHitbox);
		public virtual void Smash(Vector2 position) { }

		public bool released = false;
		public double radians = 0;

		private float _angularMomentum = 1;
		private int _lingerTimer = 0;
		private int _flickerTime = 0;
		private bool _statBuffed = false;

		public SpriteEffects Effects => ((Main.player[projectile.owner].direction * (int)Main.player[projectile.owner].gravDir) < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		public float TrueRotation => ((float)radians + 3.9f) + ((Effects == SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);
		public Vector2 Origin => (Effects == SpriteEffects.FlipHorizontally) ? new Vector2(Size, Size) : new Vector2(0, Size);

		public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor;
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, Size, Size), color, TrueRotation, Origin, projectile.scale, Effects, 0);
			SafeDraw(spriteBatch, lightColor);
			if (projectile.ai[0] >= ChargeTime && !released && _flickerTime < 16)
			{
				_flickerTime++;
				color = Color.White;
				float flickerTime2 = (_flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0)
					alpha = 0;

				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, Size, Size, Size), color * alpha, TrueRotation, Origin, projectile.scale, Effects, 1);
			}
			return false;
		}

		public sealed override bool PreAI()
		{
			SafeAI();

			projectile.scale = projectile.ai[0] < 10 ? (projectile.ai[0] / 10f) : 1;
			Player player = Main.player[projectile.owner];
			player.heldProj = projectile.whoAmI;

			if (player.HeldItem.useTurn)
			{
				player.direction = Math.Sign(player.velocity.X);
				if (player.direction == 0)
					player.direction = player.oldDirection;
			}

			int degrees = (int)((player.itemAnimation * -0.7) + 55) * player.direction * (int)player.gravDir;
			if (player.direction == 1)
				degrees += 180;

			if (!_statBuffed)
			{
				_statBuffed = true;

				float damageMult = player.meleeDamage;
				float damageFlat = 0;
				float add = 0;
				PlayerHooks.ModifyWeaponDamage(player, player.HeldItem, ref add, ref damageMult, ref damageFlat);

				minDamage = (int)(minDamage * damageMult) + (int)damageFlat;
				maxDamage = (int)(maxDamage * (1 + ((damageMult - 1) / 6))) + (int)damageFlat;
			}

			radians = degrees * (Math.PI / 180);
			if (player.channel && !released)
			{
				if (projectile.ai[0] == 0)
				{
					player.itemTime = 180;
					player.itemAnimation = 180;
				}
				if (projectile.ai[0] < ChargeTime)
				{
					projectile.ai[0]++;
					float rot = Main.rand.NextFloat(MathHelper.TwoPi);
					if (DustType != -1)
						Dust.NewDustPerfect(projectile.Center + Vector2.One.RotatedBy(rot) * 35, DustType, -Vector2.One.RotatedBy(rot) * 1.5f, 0, default, projectile.ai[0] / 100f);
					if (projectile.ai[0] < ChargeTime / 1.5f || projectile.ai[0] % 2 == 0)
						_angularMomentum = -1;
					else
						_angularMomentum = 0;
				}
				else
				{
					if (projectile.ai[0] == ChargeTime)
					{
						for (int k = 0; k <= 100; k++)
						{
							if (DustType != -1)
								Dust.NewDustPerfect(projectile.Center, DustType, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2), 0, default, 1.5f);
						}
						Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
						projectile.ai[0]++;
					}
					if (DustType != -1)
						Dust.NewDustPerfect(projectile.Center, DustType, Vector2.One.RotatedByRandom(MathHelper.TwoPi));

					_angularMomentum = 0;
				}
				projectile.damage = (int)((minDamage + (int)((projectile.ai[0] / ChargeTime) * (maxDamage - minDamage))) * player.meleeDamage);
				projectile.knockBack = MinKnockback + (int)((projectile.ai[0] / ChargeTime) * (MaxKnockback - MinKnockback));
			}
			else
			{
				projectile.scale = 1;

				if (_angularMomentum < MaxSpeed)
					_angularMomentum += Acceleration;

				if (!released)
				{
					released = true;
					projectile.friendly = true;
					Main.PlaySound(SoundID.Item1, projectile.Center);
				}
			}

			projectile.position.Y = player.Center.Y - (int)(Math.Sin(radians * 0.96) * Size) - (projectile.height / 2);
			projectile.position.X = player.Center.X - (int)(Math.Cos(radians * 0.96) * Size) - (projectile.width / 2);
			if (_lingerTimer == 0)
			{
				player.itemTime++;
				player.itemAnimation++;
				if (player.itemTime > _angularMomentum + 1)
				{
					player.itemTime -= (int)_angularMomentum;
					player.itemAnimation -= (int)_angularMomentum;
				}
				else
				{
					player.itemTime = 2;
					player.itemAnimation = 2;
				}
				if (player.itemTime == 2 || (Main.tile[(int)projectile.Center.X / 16, (int)((projectile.Center.Y + 24) / 16)].collisionType == 1 && released))
				{
					_lingerTimer = 30;

					if (projectile.ai[0] >= ChargeTime)
						Smash(projectile.Center);

					if (Main.tile[(int)projectile.Center.X / 16, (int)((projectile.Center.Y + 24) / 16)].collisionType == 1)
						player.GetModPlayer<MyPlayer>().Shake += (int)(projectile.ai[0] * 0.2f);

					projectile.friendly = false;
					Main.PlaySound(SoundID.Item70, projectile.Center);
					Main.PlaySound(SoundID.NPCHit42, projectile.Center);
				}
			}
			else
			{
				_lingerTimer--;
				if (_lingerTimer == 1)
				{
					projectile.active = false;
					player.itemTime = 2;
					player.itemAnimation = 2;
				}
				player.itemTime++;
				player.itemAnimation++;
			}
			return true;
		}
	}
}
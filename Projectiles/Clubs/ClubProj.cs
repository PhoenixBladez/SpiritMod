using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = Projectile.height = 48;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			SafeSetDefaults();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].Center, Projectile.Center) ? true : base.Colliding(projHitbox, targetHitbox);
		public virtual void Smash(Vector2 position) { }

		public bool released = false;
		public double radians = 0;

		private float _angularMomentum = 1;
		private int _lingerTimer = 0;
		private int _flickerTime = 0;
		private bool _statBuffed = false;

		public SpriteEffects Effects => ((Main.player[Projectile.owner].direction * (int)Main.player[Projectile.owner].gravDir) < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		public float TrueRotation => ((float)radians + 3.9f) + ((Effects == SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);
		public Vector2 Origin => (Effects == SpriteEffects.FlipHorizontally) ? new Vector2(Size, Size) : new Vector2(0, Size);

		public sealed override bool PreDraw(ref Color lightColor)
		{
			Color color = lightColor;
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, Size, Size), color, TrueRotation, Origin, Projectile.scale, Effects, 0);
			SafeDraw(Main.spriteBatch, lightColor);
			if (Projectile.ai[0] >= ChargeTime && !released && _flickerTime < 16)
			{
				_flickerTime++;
				color = Color.White;
				float flickerTime2 = (_flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0)
					alpha = 0;

				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, Size, Size, Size), color * alpha, TrueRotation, Origin, Projectile.scale, Effects, 1);
			}
			return false;
		}

		public sealed override bool PreAI()
		{
			SafeAI();

			Projectile.scale = Projectile.ai[0] < 10 ? (Projectile.ai[0] / 10f) : 1;
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;

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

				PlayerLoader.ModifyWeaponDamage(player, player.HeldItem, ref player.GetDamage(DamageClass.Melee));

				minDamage = (int)player.GetDamage(DamageClass.Melee).ApplyTo(minDamage);
				maxDamage = (int)player.GetDamage(DamageClass.Melee).ApplyTo(maxDamage);
			}

			radians = degrees * (Math.PI / 180);
			if (player.channel && !released)
			{
				if (Projectile.ai[0] == 0)
				{
					player.itemTime = 180;
					player.itemAnimation = 180;
				}
				if (Projectile.ai[0] < ChargeTime)
				{
					Projectile.ai[0]++;
					float rot = Main.rand.NextFloat(MathHelper.TwoPi);
					if (DustType != -1)
						Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedBy(rot) * 35, DustType, -Vector2.One.RotatedBy(rot) * 1.5f, 0, default, Projectile.ai[0] / 100f);
					if (Projectile.ai[0] < ChargeTime / 1.5f || Projectile.ai[0] % 2 == 0)
						_angularMomentum = -1;
					else
						_angularMomentum = 0;
				}
				else
				{
					if (Projectile.ai[0] == ChargeTime)
					{
						for (int k = 0; k <= 100; k++)
						{
							if (DustType != -1)
								Dust.NewDustPerfect(Projectile.Center, DustType, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2), 0, default, 1.5f);
						}
						SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
						Projectile.ai[0]++;
					}
					if (DustType != -1)
						Dust.NewDustPerfect(Projectile.Center, DustType, Vector2.One.RotatedByRandom(MathHelper.TwoPi));

					_angularMomentum = 0;
				}
				float dmg = (minDamage + ((Projectile.ai[0] / ChargeTime) * (maxDamage - minDamage)));
				Projectile.damage = (int)player.GetDamage(DamageClass.Melee).ApplyTo(dmg);
				Projectile.knockBack = MinKnockback + (int)((Projectile.ai[0] / ChargeTime) * (MaxKnockback - MinKnockback));
			}
			else
			{
				Projectile.scale = 1;

				if (_angularMomentum < MaxSpeed)
					_angularMomentum += Acceleration;

				if (!released)
				{
					released = true;
					Projectile.friendly = true;
					SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
				}
			}

			Projectile.position.Y = player.Center.Y - (int)(Math.Sin(radians * 0.96) * Size) - (Projectile.height / 2);
			Projectile.position.X = player.Center.X - (int)(Math.Cos(radians * 0.96) * Size) - (Projectile.width / 2);
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

				Tile tile = Main.tile[(int)Projectile.Center.X / 16, (int)((Projectile.Center.Y + 24) / 16)];
				bool validTile = tile.HasTile && tile.BlockType == BlockType.Solid && Main.tileSolid[tile.TileType];
				if (player.itemTime == 2 || (validTile && released))
				{
					_lingerTimer = 30;

					if (Projectile.ai[0] >= ChargeTime)
						Smash(Projectile.Center);

					if (Main.tile[(int)Projectile.Center.X / 16, (int)((Projectile.Center.Y + 24) / 16)].BlockType == BlockType.Solid)
						player.GetModPlayer<MyPlayer>().Shake += (int)(Projectile.ai[0] * 0.2f);

					Projectile.friendly = false;
					SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
					SoundEngine.PlaySound(SoundID.NPCHit42, Projectile.Center);
				}
			}
			else
			{
				_lingerTimer--;
				if (_lingerTimer == 1)
				{
					Projectile.active = false;
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
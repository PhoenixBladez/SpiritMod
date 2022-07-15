using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops.ScarabPet
{
	internal class ScarabPetProjectile : ModProjectile
	{
		const float GROUND_MOVE_SPEED = 6f;

		private Player Owner => Main.player[Projectile.owner];
		private bool HasJumped { get => Projectile.ai[0] != 0; set => Projectile.ai[0] = value ? 1 : 0; }
		private bool Jumping { get => Projectile.ai[1] != 0; set => Projectile.ai[1] = value ? 1 : 0; }

		private int _frame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Scarab");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 66;
			Projectile.height = 44;

			AIType = 0;
		}

		public override void AI()
		{
			float dist = Projectile.DistanceSQ(Owner.Center);

			if (dist < 300 * 300)
				NearbyMovement();
			else if (dist < 600 * 600)
				FollowPlayerGround();
			else
				FollowPlayerFlight();

			Projectile.frame = _frame;

			if (Projectile.velocity.X > 0)
				Projectile.spriteDirection = -1;
			else if (Projectile.velocity.X < 0)
				Projectile.spriteDirection = 1;
		}

		private void NearbyMovement()
		{
			HasJumped = false;
			Jumping = false;

			_frame = 0;

			Projectile.velocity.X *= 0.95f;
			Projectile.velocity.Y += 0.2f;

			float throwaway = 6;
			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref throwaway, ref Projectile.gfxOffY);
		}

		private void FollowPlayerGround()
		{
			Projectile.velocity.Y += 0.2f;

			if (!HasJumped)
			{
				HasJumped = true;
				Jumping = true;

				Projectile.velocity.Y = -5f;
			}
			else
			{
				if (!Jumping)
				{
					float throwaway = 6;
					Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref throwaway, ref Projectile.gfxOffY);

					if (Owner.Center.X < Projectile.Center.X)
						Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -GROUND_MOVE_SPEED, 0.15f);
					else
						Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, GROUND_MOVE_SPEED, 0.15f);
				}
				else
				{
					if (Projectile.velocity.Y >= 0)
						_frame = 1;
				}
			}
		}

		private void FollowPlayerFlight() => Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Owner.Center) * 12, 0.05f);

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Jumping = false;
			return false;
		}
	}
}

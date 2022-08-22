using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.OccultistDrops.OccultistPet
{
	public class OccultistPetProjectile : ModProjectile
	{
		const float GROUND_MOVE_SPEED = 4f;

		private Player Owner => Main.player[Projectile.owner];
		private bool Head { get => Projectile.ai[0] == 1; set => Projectile.ai[0] = value ? 1 : 0; }
		private ref float HeadTime => ref Projectile.ai[1];

		private Vector2 HeadPosition => Owner.Center.ToPoint().ToVector2() - new Vector2(10 + (Projectile.spriteDirection == 1 ? -6 : -10), 50 - Owner.gfxOffY);

		private bool _trySetHead = false;
		private bool _useHeadPos = false;
		private int _state = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Occultist");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 24;
			Projectile.height = 56;
			Projectile.light = 0;

			AIType = 0;
		}

		public override void AI()
		{
			Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>().PetFlag(Projectile);

			if (_state == 0)
			{
				if (!Head)
					FollowPlayerGround();
				else
					SitOnHead();
			}
			else
				TryTeleport();

			if (!Head)
			{
				if (Projectile.velocity.X <= 0)
					Projectile.spriteDirection = -1;
				else
					Projectile.spriteDirection = 1;
			}
		}

		private void SitOnHead()
		{
			Projectile.frameCounter++;

			int offset = Projectile.frameCounter / 4;
			Projectile.frame = offset + 23;
			if (Projectile.frame > 24)
				Projectile.frame = 24;

			Projectile.spriteDirection = Owner.direction;
			Projectile.Center = HeadPosition;
			Projectile.velocity = Vector2.Zero;

			HeadTime++;

			if (HeadTime > 470)
				Projectile.frame = 25;
			if (HeadTime > 480)
			{
				Head = false;
				Projectile.velocity = new Vector2(Main.rand.NextFloat(-2f, 2), Main.rand.NextFloat(-3f, -2f));
				ResetState(0);
			}
		}

		private void FollowPlayerGround()
		{
			Projectile.velocity.Y += 0.2f;

			float dist = Owner.DistanceSQ(Projectile.Center);
			float throwaway = 6;
			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref throwaway, ref Projectile.gfxOffY);

			if (dist > 120 * 120)
			{
				if (Owner.Center.X < Projectile.Center.X)
					Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -GROUND_MOVE_SPEED, 0.03f);
				else
					Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, GROUND_MOVE_SPEED, 0.03f);
			}
			else
				Projectile.velocity.X *= 0.9f;

			if (Math.Abs(Projectile.velocity.X) > 0.05)
			{
				Projectile.frameCounter++;
				int offset = Math.Abs(Projectile.frameCounter % (5 * 3)) / 4; //Walk loop
				Projectile.frame = offset;
			}
			else
			{
				Projectile.frameCounter = 0;
				Projectile.frame = 0;
			}

			if (dist > 700 * 700)
				ResetState(1);
		}

		private void TryTeleport()
		{
			Projectile.frameCounter++;
			Projectile.velocity *= 0.98f;

			int offset = Projectile.frameCounter / 4;
			Projectile.frame = offset;

			if (Projectile.frame == 18)
			{
				if (_trySetHead)
				{
					if (Projectile.DistanceSQ(Owner.Center) > 1000 * 1000)
					{
						Head = true;
						_useHeadPos = true;
					}
					else
					{
						Head = false;
						HeadTime = 0;
						_useHeadPos = false;
					}

					_trySetHead = false;
				}

				if (!Head)
					FindTeleportSpot();
			}
			else if (Projectile.frame >= 22)
			{
				ResetState(0);
				_trySetHead = true;
				_useHeadPos = false;
			}

			if (_useHeadPos)
				Projectile.Center = HeadPosition;
		}

		private void FindTeleportSpot()
		{
			Vector2 origin = Owner.Center + Owner.velocity * 6;
			Projectile.Center = origin;
		}

		private void ResetState(int newState)
		{
			_state = newState;

			Projectile.frameCounter = 0;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool PreDraw(ref Color lightColor)
		{
			const int FrameWidth = 44;
			const int FrameHeight = 60;

			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			Rectangle rect = new Rectangle(0, 0, 42, FrameHeight);
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			if (Projectile.frame <= 4)
				rect.Y = Projectile.frame * FrameHeight;
			else if (Projectile.frame >= 5 && Projectile.frame <= 17)
			{
				rect.X = FrameWidth;
				rect.Y = (Projectile.frame - 5) * FrameHeight;
			}
			else if (Projectile.frame >= 18 && Projectile.frame <= 22)
			{
				rect.X = FrameWidth * 2;
				rect.Y = (Projectile.frame - 18) * FrameHeight;
			}
			else
			{
				rect.X = FrameWidth * 3;
				rect.Y = (Projectile.frame - 23) * FrameHeight;
			}

			Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition - new Vector2(8, 0), rect, lightColor, Projectile.rotation, Vector2.Zero, 1f, effect, 0);
			return false;
		}
	}
}

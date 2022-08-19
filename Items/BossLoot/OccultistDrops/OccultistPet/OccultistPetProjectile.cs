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
			Projectile.width = 44;
			Projectile.height = 56;
			Projectile.light = 0;

			AIType = 0;
		}

		public override void AI()
		{
			Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>().PetFlag(Projectile);

			if (_state == 0)
				FollowPlayerGround();
			else
				TryTeleport();

			if (Projectile.velocity.X <= 0)
				Projectile.spriteDirection = -1;
			else 
				Projectile.spriteDirection = 1;
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
				int offset = Math.Abs(Projectile.frameCounter % (7 * 3)) / 3; //Walk loop
				Projectile.frame = offset;
			}
			else
			{
				Projectile.frameCounter = 0;
				Projectile.frame = 0;
			}

			if (dist > 1000 * 1000)
				ResetState(1);
		}

		private void TryTeleport()
		{
			Projectile.frameCounter++;

			int offset = Projectile.frameCounter / 3;
			Projectile.frame = offset;

			if (Projectile.frame == 18)
			{
				Head = Main.rand.NextBool(4);

				if (Head)
					Projectile.Center = Owner.Center - new Vector2(0, 20);
				else
					FindTeleportSpot();
			}
		}

		private void FindTeleportSpot()
		{
			Vector2 origin = Owner.Center + Owner.velocity * 6;


		}

		private void ResetState(int newState)
		{
			_state = newState;

			Projectile.frameCounter = 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool PreDraw(ref Color lightColor)
		{
			const int FrameHeight = 60;

			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			Rectangle rect = new Rectangle(0, 0, 42, FrameHeight);
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			if (Projectile.frame <= 4)
				rect.Y = Projectile.frame * FrameHeight;
			else if (Projectile.frame >= 5 && Projectile.frame <= 17)
			{
				rect.X = 42;
				rect.Y = (Projectile.frame - 4) * FrameHeight;
			}
			else if (Projectile.frame >= 9 && Projectile.frame <= 11)
			{
				rect.X = 84;
				rect.Y = (Projectile.frame - 9) * FrameHeight;
			}
			else
			{
				rect.X = 126;
				rect.Y = (Projectile.frame - 12) * FrameHeight;
			}

			Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, rect, lightColor, Projectile.rotation, Vector2.Zero, 1f, effect, 0);
			return false;
		}
	}
}

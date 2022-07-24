using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	internal class VinewrathPetProjectile : ModProjectile
	{
		private Player Owner => Main.player[Projectile.owner];
		private ref float MoveSpeedX => ref Projectile.ai[0];
		private ref float MoveSpeedY => ref Projectile.ai[1];

		private int _state = 0;
		private int _charge = 0;

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
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.light = 0f;
			Projectile.tileCollide = false;

			AIType = 0;
		}

		public override void AI()
		{
			var modPlayer = Owner.GetModPlayer<GlobalClasses.Players.PetPlayer>();

			if (Owner.dead)
				modPlayer.vinewrathPet = false;

			if (modPlayer.vinewrathPet)
				Projectile.timeLeft = 2;

			if (_state == 0)
				NearbyMovement();
			else
				FollowPlayerFlight();

			if (Projectile.velocity.X > 0)
				Projectile.spriteDirection = -1;
			else if (Projectile.velocity.X < 0)
				Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.X * 0.04f;
		}

		private void NearbyMovement()
		{
			GeneralMovement();

			if (Projectile.DistanceSQ(Owner.Center) > 700 * 700)
				ResetState(1);
		}

		/// <summary>Literally stolen from ReachBoss1.cs</summary>
		/// <param name="player"></param>
		public void GeneralMovement()
		{
			Player player = Owner;

			if (Projectile.Center.X >= player.Center.X && MoveSpeedX >= -30) // flies to players x position
				MoveSpeedX--;
			else if (Projectile.Center.X <= player.Center.X && MoveSpeedX <= 30)
				MoveSpeedX++;

			Projectile.velocity.X = MoveSpeedX * 0.08f;

			float sine = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) * 60f;
			if (Projectile.Center.Y >= player.Center.Y - 60 + sine && MoveSpeedY >= -14) //Flies to players Y position
				MoveSpeedY--;
			else if (Projectile.Center.Y <= player.Center.Y - 60f + sine && MoveSpeedY <= 14)
				MoveSpeedY++;
			Projectile.velocity.Y = MoveSpeedY * 0.1f;
		}

		private void FollowPlayerFlight()
		{
			const float MaxSpeedDistance = 800;
			const float DefaultSpeed = 7f;
			const int StartCharge = 40;
			const int MaxCharge = 50;

			_charge++;

			float dist = Projectile.DistanceSQ(Main.player[Projectile.owner].Center);
			float magnitude = DefaultSpeed;

			if (dist > MaxSpeedDistance * MaxSpeedDistance)
				magnitude = DefaultSpeed + (((float)Math.Sqrt(dist) - MaxSpeedDistance) * 0.05f);

			if (_charge < StartCharge)
				Projectile.velocity *= 0.8f;
			else if (_charge > StartCharge && _charge < MaxCharge)
			{
				float mult = (_charge - (float)StartCharge) / MaxCharge;
				Projectile.velocity = Projectile.DirectionTo(Owner.Center) * mult * magnitude;
			}
			else if (_charge >= MaxCharge)
				Projectile.velocity = Projectile.DirectionTo(Owner.Center) * magnitude;

			if (dist <= 100 * 100)
			{
				_charge = 0;

				MoveSpeedX = Projectile.velocity.X / 0.08f;
				MoveSpeedY = Projectile.velocity.Y / 0.1f;
				ResetState(0);
			}
		}

		private void ResetState(int newState)
		{
			_state = newState;

			Projectile.frameCounter = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			Rectangle rect = new Rectangle(0, 0, 40, 40);
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			if (Projectile.frame <= 3)
				rect.Y = Projectile.frame * 42;
			else if (Projectile.frame >= 4 && Projectile.frame <= 8)
			{
				rect.X = 42;
				rect.Y = (Projectile.frame - 4) * 42;
			}
			else if (Projectile.frame >= 9 && Projectile.frame <= 11)
			{
				rect.X = 84;
				rect.Y = (Projectile.frame - 9) * 42;
			}
			else
			{
				rect.X = 126;
				rect.Y = (Projectile.frame - 12) * 42;
			}

			Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, rect, lightColor, Projectile.rotation, Vector2.Zero, 1f, effect, 0);
			return false;
		}
	}
}

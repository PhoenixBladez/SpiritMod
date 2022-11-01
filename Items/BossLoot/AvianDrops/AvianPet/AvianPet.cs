using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AvianDrops.AvianPet
{
	public class AvianPet : ModProjectile
	{
		private Player Owner => Main.player[Projectile.owner];
		private Vector2 RollOrigin => Projectile.frame == 0 ? new Vector2(11, 13) : new Vector2(20, 17);

		public ref float State => ref Projectile.ai[0];
		public ref float TargetNPC => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Hatchling");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 22;
			Projectile.height = 26;
			Projectile.light = 0;
			Projectile.tileCollide = true;

			AIType = 0;
		}

		public override void AI()
		{
			Owner.GetModPlayer<GlobalClasses.Players.PetPlayer>().PetFlag(Projectile);

			if (State == 0)
				Follow();
			else if (State == 1)
				Relax();
		}

		private void Relax()
		{
			Projectile.rotation *= 0.92f;
			Projectile.velocity *= 0.9f;

			if (Math.Abs(Projectile.rotation) < 0.002f)
				Projectile.frameCounter++;

			if (Projectile.frameCounter > 5)
			{
				if (Projectile.frame == 0)
					Projectile.frame = 2;
				else if (Projectile.frame < 10)
					Projectile.frame++;

				Projectile.frameCounter = 0;
			}

			if (Projectile.DistanceSQ(Owner.Center) > 200 * 200)
				State = 0;
		}

		private void Follow()
		{
			const float MaxFollowSpeed = 7;

			float targetX = Owner.Center.X;

			Projectile.frame = 0;

			if (Math.Abs(Projectile.Center.X - Owner.Center.X) < 150)
			{
				Projectile.velocity.X *= 0.95f;

				if (Math.Abs(Projectile.velocity.X) < 0.2f)
					State = 1;
			}
			else
				Projectile.velocity.X += Projectile.Center.X < targetX ? 0.1f : -0.1f;

			Projectile.velocity.Y += 0.2f;

			if (Math.Abs(Projectile.velocity.X) > MaxFollowSpeed)
				Projectile.velocity.X = Projectile.velocity.X < 0 ? -MaxFollowSpeed : MaxFollowSpeed;

			Projectile.rotation += Projectile.velocity.X * 0.03f;

			float throwaway = 6;
			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref throwaway, ref Projectile.gfxOffY);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			const int FrameOffsetY = 36;

			Rectangle source = new Rectangle(0, FrameOffsetY * Projectile.frame, 40, 34);

			if (Projectile.frame == 0)
				source = new(8, 8, 22, 26);

			if (Projectile.frame > 1 && Projectile.frame <= 10)
			{
				source.X = 34;
				source.Y = FrameOffsetY * (Projectile.frame - 2);
			}

			Vector2 drawPos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;

			if (Projectile.frame == 0)
			{
				float sineY = (float)Math.Pow(Math.Sin(Projectile.rotation), 2) * 4;
				drawPos.Y += 2 + sineY;
			}

			Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, drawPos, source, lightColor, Projectile.rotation, RollOrigin, Vector2.One, SpriteEffects.None, 0);
			return false;
		}
	}
}

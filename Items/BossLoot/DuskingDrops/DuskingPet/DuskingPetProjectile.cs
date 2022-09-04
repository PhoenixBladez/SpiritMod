using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.DuskingDrops.DuskingPet
{
	public class DuskingPetProjectile : ModProjectile
	{
		private Player Owner => Main.player[Projectile.owner];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Occultist");
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 24;
			Projectile.height = 56;
			Projectile.light = 0;
			Projectile.tileCollide = false;

			AIType = 0;
		}

		public override void AI()
		{
			Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>().PetFlag(Projectile);
			FollowPlayer();
			Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;

			if (Main.rand.NextBool(13))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
		}

		private void FollowPlayer()
		{
			const float MaxSpeed = 10;

			Projectile.frameCounter++;
			Projectile.frame = (Projectile.frameCounter / 5) % 5;

			Projectile.velocity += Projectile.DirectionTo(Owner.Center + Owner.velocity) * 0.5f;

			if (Projectile.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed;

			Lighting.AddLight(Projectile.Center, 2f, 0.2f, 1.9f);

			if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 155, 0.1f);
			else
				Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 0, 0.1f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
	}
}

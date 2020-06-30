using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Projectiles.Thrown
{
	public class SlidingIce : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sliding Ice");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.magic = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 40; i++) {
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 39, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 68);
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
		}
		int direction = 0; //0 is left, 1 is right
		int jumpCounter = 6;
		public override bool PreAI()
		{
			jumpCounter++;
			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 1.005F;
			projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -7, 7);
			if(projectile.velocity.X > 0) {
				direction = 1;
			}
			if(projectile.velocity.X < 0) {
				direction = 0;
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(oldVelocity.X != projectile.velocity.X) {
				if(jumpCounter > 5) {
					jumpCounter = 0;
					projectile.position.Y -= 20;
					projectile.velocity.X = oldVelocity.X;
				} else {
					//   projectile.position.Y += 12;
					return true;
				}
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

	}
}

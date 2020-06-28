using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class FeatherArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather Arrow");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Vector2 vel = new Vector2(2, 0).RotatedBy((float)(Main.rand.Next(90) * Math.PI / 180));
			Projectile.NewProjectile(projectile.Center, vel, ModContent.ProjectileType<FeatherFrag>(), projectile.damage, 0, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center, vel.RotatedBy(1.57), ModContent.ProjectileType<FeatherFrag>(), projectile.damage, 0, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center, vel.RotatedBy(3.14), ModContent.ProjectileType<FeatherFrag>(), projectile.damage, 0, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center, vel.RotatedBy(4.71), ModContent.ProjectileType<FeatherFrag>(), projectile.damage, 0, Main.myPlayer);
			return true;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 14;
			projectile.penetrate = 1;
			projectile.height = 14;
		}
		public override void AI()
		{
			int num = 5;
			for(int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 42, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
			for(int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 42, 0, 0);
			}

		}
	}
}

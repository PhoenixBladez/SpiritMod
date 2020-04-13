using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Boss
{
	public class BoneWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Wave");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 48;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 1000;
			projectile.tileCollide = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}
		public override void AI()
		{
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10;
            projectile.velocity.X *= 1.005f;
			Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 0, default(Color), 1f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 0, default(Color), 1)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
		}
	}
}
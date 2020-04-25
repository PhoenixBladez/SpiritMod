using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Dragon
{
    public class DragonHeadTwo : ModProjectile
    {
		int counter = 0;
		float distance = 4;
		int rotationalSpeed = 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon");
		}
        public override void SetDefaults()
        {
            projectile.penetrate = 6;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 190;
			projectile.damage = 13;
            projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
		 public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;
		}
		public override void AI()
		{
			projectile.spriteDirection = 1;
			if (projectile.ai[0] > 0)
			{
			 projectile.spriteDirection = 0;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.015f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
		}
	}
}
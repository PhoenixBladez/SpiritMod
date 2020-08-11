using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class SineBall : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		bool initialized = false;
		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
		{

			if (!initialized) 
			{
				initialSpeed = projectile.velocity;
				initialized = true;
			}
			projectile.spriteDirection = 1;
			if (projectile.ai[0] > 0) {
				projectile.spriteDirection = 0;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.1f;
			projectile.ai[0] += rotationalSpeed;
			
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(projectile.ai[0] * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
		}
	}
}
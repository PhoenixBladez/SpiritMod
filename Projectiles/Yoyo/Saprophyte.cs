using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class Saprophyte : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saprophyte");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(546);
			projectile.extraUpdates = 1;
			aiType = 546;
		}

		public override bool PreAI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 60)
			{
				Projectile.NewProjectile(projectile.position.X + (projectile.width * .5f), projectile.position.Y + (projectile.height * .5f), projectile.velocity.X, projectile.velocity.Y, 131, projectile.damage >> 1, 0f, projectile.owner, 0f, 0f);
				projectile.frameCounter = 0;
			}
			return true;
		}

		public override void AI()
		{
			projectile.rotation -= 10f;
		}
	}
}

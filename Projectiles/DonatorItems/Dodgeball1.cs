using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.DonatorItems
{
	public class Dodgeball1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dodgeball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
			projectile.damage = 24;
			projectile.extraUpdates = 1;
			projectile.width = 30;
			projectile.height = 30;
			aiType = ProjectileID.WoodenBoomerang;
			projectile.penetrate = -1;
		}

	}
}

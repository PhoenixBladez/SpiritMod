using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class RoyalP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Roll");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodYoyo);
			projectile.damage = 13;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.WoodYoyo;
		}
	}
}

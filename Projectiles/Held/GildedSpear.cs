using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class GildedSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Impaler");
		}

		public override void SetDefaults()
		{
			projectile.height = 10;
			projectile.width = 30;
			projectile.timeLeft = 30;
		}

	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class ElectricChain : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ElectricChain");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 16;
			projectile.melee = true;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 2;
			projectile.tileCollide = false;
			projectile.extraUpdates = 7;
		}
	}
}
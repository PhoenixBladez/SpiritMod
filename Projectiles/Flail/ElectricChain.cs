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
			Projectile.hostile = false;
			Projectile.width = 16;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 7;
		}
	}
}
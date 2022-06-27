using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class Omniwrench : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omniwrench");
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;

			Projectile.aiStyle = 3;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 6;
			Projectile.light = 0.3f;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.PossessedHatchet;
			Projectile.tileCollide = false;
		}


	}
}

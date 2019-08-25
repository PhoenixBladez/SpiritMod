using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class Omniwrench : ModProjectile
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omniwrench");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;

			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 6;
			projectile.light = 0.3f;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.PossessedHatchet;
			projectile.tileCollide = false;
		}


	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class SporeExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exploding Spore");
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.timeLeft = 10;
			Projectile.height = 150;
			Projectile.penetrate = 3;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
		}
	}
}

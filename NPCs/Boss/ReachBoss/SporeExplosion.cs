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
			projectile.width = 50;
			projectile.timeLeft = 10;
			projectile.height = 50;
			projectile.penetrate = 3;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
		}
	}
}

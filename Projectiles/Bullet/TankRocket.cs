using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class TankRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RocketI V2");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.RocketI);
			aiType = ProjectileID.RocketI;
			projectile.hostile = false;
			projectile.friendly = true;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.RocketI;
			return true;
		}
	}
}
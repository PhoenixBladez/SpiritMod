using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Boss
{
	public class BoneWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Wave");
		}

		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.height = 54;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 1000;
			projectile.tileCollide = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

	}
}
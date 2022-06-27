using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	public class DesertFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desert Feather");
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 1000;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height,
					DustID.Dirt, 0, 1, 133);
			}
		}
	}
}
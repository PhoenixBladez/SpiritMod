using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class GlitchLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glitch Laser");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 90;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
		}
	}
}
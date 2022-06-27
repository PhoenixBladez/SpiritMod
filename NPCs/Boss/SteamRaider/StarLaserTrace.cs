using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class StarLaserTrace : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Laser");

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(40, 111, 153) * .3f), new RoundCap(), new DefaultTrailPosition(), 10f, 1550f);

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
		}
	}
}
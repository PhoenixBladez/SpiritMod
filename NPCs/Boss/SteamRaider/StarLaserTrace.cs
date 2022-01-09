using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class StarLaserTrace : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Laser");

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(projectile, new StandardColorTrail(new Color(40, 111, 153) * .3f), new RoundCap(), new DefaultTrailPosition(), 10f, 1550f);

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}
	}
}
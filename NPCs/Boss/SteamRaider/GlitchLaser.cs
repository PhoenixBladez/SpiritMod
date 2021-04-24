using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class GlitchLaser : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Glitch Laser");

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
			projectile.tileCollide = true;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new StandardColorTrail(new Color(255, 232, 82)), new RoundCap(), new DefaultTrailPosition(), 10f, 450f);

		public override void AI()
		{
			if (projectile.velocity.Length() < 32)
				projectile.velocity *= 1.02f;
			else
				projectile.velocity = Vector2.Normalize(projectile.velocity) * 32;
		}
	}
}
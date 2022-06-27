using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class GlitchLaser : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Glitch Laser");

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = true;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 232, 82)), new RoundCap(), new DefaultTrailPosition(), 10f, 450f);

		public override void AI()
		{
			if (Projectile.velocity.Length() < 32)
				Projectile.velocity *= 1.02f;
			else
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 32;
		}
	}
}
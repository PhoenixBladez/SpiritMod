using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AdamantiteStaffProj2 : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Adamantite Blast");

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 80;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			float trailwidth = 20;
			float traillength = 150;
			tManager.CreateTrail(projectile, new StandardColorTrail(new Color(252, 3, 57) * 0.9f), new RoundCap(), new DefaultTrailPosition(), trailwidth / 2, traillength * 0.8f);
			tManager.CreateTrail(projectile, new StandardColorTrail(new Color(255, 255, 255)), new RoundCap(), new DefaultTrailPosition(), trailwidth / 3, traillength * 0.75f);
			tManager.CreateTrail(projectile, new GradientTrail(new Color(252, 3, 57) * 0.75f, new Color(255, 201, 213) * 0.75f), new RoundCap(), new DefaultTrailPosition(), trailwidth, traillength);
		}

		public override bool PreAI()
		{
			/*int num = 5;
			for (int k = 0; k < 10; k++) {
				int index2 = Dust.NewDust(projectile.position, 10, 10, 60, 0.0f, 0.0f, 0, new Color(), 1.3f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}*/
			projectile.velocity = projectile.velocity.RotatedBy(System.Math.PI / 40);
			return true;
		}

	}
}

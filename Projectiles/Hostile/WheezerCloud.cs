using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class WheezerCloud : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gas Cloud");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.extraUpdates = 1;
			projectile.light = 0;
			projectile.timeLeft = 255;
			aiType = ProjectileID.Bullet;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.scale *= .8f;
		}

		public override void AI()
		{
			projectile.velocity *= .98f;

			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if(projectile.frameCounter >= 6) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if(projectile.frame >= 8)
					projectile.frame = 0;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(2) == 1)
				target.AddBuff(BuffID.Poisoned, 300);
		}
	}
}
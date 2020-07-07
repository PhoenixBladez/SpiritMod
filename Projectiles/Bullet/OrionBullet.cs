
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class OrionBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion Bullet");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 300;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int timer = 1;
		public override void AI()
		{
			timer--;
			for(int i = 0; i < 6; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 187);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
			if(timer == 0) {
				Projectile.NewProjectile(projectile.Center, projectile.velocity,
					ModContent.ProjectileType<StarTrail>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
				timer = 30;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, projectile.position, 3);
		}
	}
}

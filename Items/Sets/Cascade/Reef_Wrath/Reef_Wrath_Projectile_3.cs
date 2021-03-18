using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Cascade.Reef_Wrath
{
	public class Reef_Wrath_Projectile_3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Reef");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 24;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.penetrate = 10;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.scale = 1f;
			projectile.alpha = 250;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int num2 = Main.rand.Next(20, 40);
			for (int index1 = 0; index1 < num2; ++index1)
			{
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 5, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Main.dust[index2].velocity *= 1.2f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].velocity += projectile.velocity;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void AI()
		{
			if (projectile.alpha > 0 && projectile.timeLeft > 50)
				projectile.alpha -= 5;
			
			if (projectile.timeLeft <= 50)
				projectile.alpha += 25;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
		}
	}
}

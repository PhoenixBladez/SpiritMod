using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class RyBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'ylheian");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;

			projectile.damage = 3;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
		}
		bool summoned = false;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if(!summoned) {
				for(int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 173, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 7f;
				}
				summoned = true;
			}
			for (int i = 0; i < 12; i++)
			{
				Dust.NewDust(projectile.Center, projectile.width, projectile.height, 173);
			}
		}
	}
}

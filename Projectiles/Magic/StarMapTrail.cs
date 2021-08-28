using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Magic
{
	public class StarMapTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Map Trail");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;

			projectile.ranged = true;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.damage = 0;
			projectile.alpha = 255;
			projectile.extraUpdates = 16;
			projectile.light = 0;
			projectile.ignoreWater = true;
		}
		public override bool PreAI()
		{
			int num = 5;
			int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.UnusedWhiteBluePurple, 0.0f, 0.0f, 0, new Color(), 1.3f);
			Main.dust[index2].position = projectile.Center - projectile.velocity / num;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;
			Main.dust[index2].noLight = true;
			return true;
		}

	}
}

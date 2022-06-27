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
			Projectile.width = 16;
			Projectile.height = 16;

			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowFriendly;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.damage = 0;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 16;
			Projectile.light = 0;
			Projectile.ignoreWater = true;
		}
		public override bool PreAI()
		{
			int num = 5;
			int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.UnusedWhiteBluePurple, 0.0f, 0.0f, 0, new Color(), 1.3f);
			Main.dust[index2].position = Projectile.Center - Projectile.velocity / num;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;
			Main.dust[index2].noLight = true;
			return true;
		}

	}
}

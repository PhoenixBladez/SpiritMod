using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class StellarDrillProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Drill");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 48;
			projectile.aiStyle = 20;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 133, projectile.velocity.X, projectile.velocity.Y);
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			return true;
		}

	}
}
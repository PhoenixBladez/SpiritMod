
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class TentacleChainProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tentacle Chain");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.melee = true;
		}
		//  bool comingHome = false;
		public override bool PreAI()
		{
			if(projectile.Hitbox.Intersects(Main.player[projectile.owner].Hitbox) && projectile.timeLeft < 870) {
				projectile.active = false;
			}
			if(projectile.timeLeft < 869) {
				Vector2 direction9 = Main.player[projectile.owner].Center - projectile.position;
				projectile.velocity = projectile.velocity.RotatedBy(direction9.ToRotation() - projectile.velocity.ToRotation());
				projectile.velocity *= 1.10113969f;
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			} else {
				projectile.velocity -= new Vector2(projectile.ai[0], projectile.ai[1]) / 30f;
				projectile.rotation = new Vector2(projectile.ai[0], projectile.ai[1]).ToRotation() - 1.57f;
			}
			return false;
		}
		//projectile.ai[0]: X speed initial
		//projectile.ai[1]: y speed initial

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
			"SpiritMod/Projectiles/Flail/TentacleChain_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}
	}
}

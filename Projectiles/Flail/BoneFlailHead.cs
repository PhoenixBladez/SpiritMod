using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class BoneFlailHead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serpent Spine");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(projectile.whoAmI);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 1);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 1);
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/BoneFlail_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}

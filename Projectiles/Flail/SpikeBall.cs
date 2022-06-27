
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class SpikeBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(Projectile.whoAmI);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(Projectile.whoAmI, oldVelocity);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawChain(Projectile.whoAmI, Main.player[Projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/FleshRender_Chain");
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}

	}
}

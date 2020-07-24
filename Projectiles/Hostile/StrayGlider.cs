using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class StrayGlider : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stray Glider");
			Main.projFrames[base.projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 46;
			projectile.height = 32;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			projectile.alpha += 5;
			if (projectile.alpha > 240) {
				projectile.active = false;
			}
		}
	}
}

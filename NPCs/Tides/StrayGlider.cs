using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class StrayGlider : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stray Glider");
			Main.projFrames[base.Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 46;
			Projectile.height = 32;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}
		public override void AI()
		{
			Projectile.alpha += 5;
			if (Projectile.alpha > 240) {
				Projectile.active = false;
			}
		}
	}
}

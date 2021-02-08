using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CobaltStaffProj1 : CobaltStaffProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Shard");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
	}
}
using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.DiseasedSlime
{
	class NoxiousIndicator : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxious Field");
			Main.projFrames[projectile.type] = 6;

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 120;
			projectile.height = 22;
			projectile.width = 22;
			projectile.alpha = 60;
		}

		public override void AI()
		{
			projectile.alpha += 2;
			projectile.velocity.Y -= 1.1f * (projectile.timeLeft / 120f);

			if (projectile.frameCounter++ >= 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
		}
	}
}

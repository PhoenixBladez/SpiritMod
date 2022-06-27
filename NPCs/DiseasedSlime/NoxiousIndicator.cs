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
			Main.projFrames[Projectile.type] = 6;

		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 120;
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.alpha = 60;
		}

		public override void AI()
		{
			Projectile.alpha += 2;
			Projectile.velocity.Y = -.9f * ((Projectile.timeLeft * 2) / 120f);
			Projectile.velocity *= .98f;

			if (Projectile.frameCounter++ >= 6)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
		}
	}
}

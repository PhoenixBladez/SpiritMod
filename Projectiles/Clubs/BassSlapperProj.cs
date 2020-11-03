using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Clubs
{
	class BassSlapperProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bass Slapper");
			Main.projFrames[projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
            Main.PlaySound(3, projectile.position, 1);
		}
		public BassSlapperProj() : base(72, 13, 26, -1, 58, 13, 27, 1.7f, 12f){}
	}
}

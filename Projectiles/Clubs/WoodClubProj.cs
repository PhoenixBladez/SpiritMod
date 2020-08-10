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
	class WoodClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Club");
			Main.projFrames[projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += (int)(projectile.ai[0] * 0.2f);
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
		}
		public WoodClubProj() : base(60, 8, 20, -1, 58, 5, 9, 1.7f, 12f){}
	}
}

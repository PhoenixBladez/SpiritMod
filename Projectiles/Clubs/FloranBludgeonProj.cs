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
	class FloranBludgeonProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Bludgeon");
			Main.projFrames[projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 32, 0, 0, ModContent.ProjectileType<BoneShockwave>(), projectile.damage / 3, projectile.knockBack / 2, projectile.owner, 8, player.direction);
		}
		public FloranBludgeonProj() : base(55, 16, 40, -1, 58, 4, 8, 2.1f, 18f){}
	}
}

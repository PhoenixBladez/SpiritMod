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
	class CryoClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Club");
			Main.projFrames[projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 150; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.CryoDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 20, 0, 0, ModContent.ProjectileType<CryoClubSpike>(), projectile.damage / 2, projectile.knockBack / 2, projectile.owner, 8, player.direction);

        }
        public CryoClubProj() : base(80, 23, 84, -1, 58, 5, 9, 1.7f, 12f){}
	}
}

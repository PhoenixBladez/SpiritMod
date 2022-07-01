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
	class SporeClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sporebreaker");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[Projectile.owner];
			for (int k = 0; k <= 110; k++) {
				Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);
			}
            Projectile.NewProjectile(Projectile.GetSource_FromAI("ClubSmash"), Projectile.Center.X + (20 * player.direction), Projectile.Center.Y - 40, 0, 0, ModContent.ProjectileType<ToxinField>(), Projectile.damage / 4, 0, Projectile.owner, 8, player.direction);

        }
        public SporeClubProj() : base(52, 22, 48, -1, 62, 6, 10, 1.9f, 16f){}
	}
}

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
			Main.projFrames[Projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[Projectile.owner];
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);
			}
            Projectile.NewProjectile(Projectile.GetSource_FromAI("ClubSmash"), Projectile.Center.X, Projectile.Center.Y - 32, 0, 0, ModContent.ProjectileType<BoneShockwave>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner, 8, player.direction);
		}
		public FloranBludgeonProj() : base(52, 19, 43, -1, 58, 4, 8, 2.1f, 18f){}
	}
}

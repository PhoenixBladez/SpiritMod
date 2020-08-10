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
	class BoneClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Club");
			Main.projFrames[projectile.type] = 3;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 32, 0, 0, ModContent.ProjectileType<BoneShockwave>(), projectile.damage / 3, projectile.knockBack / 2, projectile.owner, 8, player.direction);
		}
		public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int size = 62;
			if (projectile.ai[0] >= chargeTime) {
				
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, (float)radians + 3.9f, new Vector2(0, size), projectile.scale, SpriteEffects.None, 1);
			}
		}
		public BoneClubProj() : base(60, 20, 80, -1, 62, 5, 9, 1.7f, 12f){}
	}
}

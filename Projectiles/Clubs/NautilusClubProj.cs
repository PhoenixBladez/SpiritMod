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
	class NautilusClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nautilobber");
			Main.projFrames[projectile.type] = 3;
		}
		public override void Smash(Vector2 position)
		{
            Player player = Main.player[projectile.owner];
            for (int k = 0; k <= 110; k++)
            {
                Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
            }
            for (int k = 0; k <= 40; k++)
            {
                Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.CryoDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<NautilusBubbleSpawner>(), projectile.damage / 2, projectile.knockBack / 2, projectile.owner, 8, player.direction);

        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
        }
        public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int size = 82;
            if (projectile.ai[0] >= chargeTime)
            {

                Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, Truerotation, Origin, projectile.scale, Effects, 1);
            }
        }
        public NautilusClubProj() : base(64, 21, 48, -1, 82, 6, 11, 1.9f, 17f){}
	}
}

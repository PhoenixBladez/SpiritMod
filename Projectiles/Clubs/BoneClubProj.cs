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
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.BoneDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
            for (int i = 0; i < 6; i++)
            {
                float rotation = (float)(Main.rand.Next(180, 361) * (Math.PI / 180));
                float rotation1 = (float)(Main.rand.Next(180, 270) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation1), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 32,
                    velocity.X * -player.direction, velocity.Y, mod.ProjectileType("BoneShard"), projectile.damage/4, projectile.knockBack, projectile.owner);
                Main.projectile[proj].velocity *= 4f;
                Main.projectile[proj].scale *= Main.rand.NextFloat(.6f, 1f);
            }
        }
		public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int size = 62;
			if (projectile.ai[0] >= chargeTime) {
				
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, Truerotation, Origin, projectile.scale, Effects, 1);
			}
		}
		public BoneClubProj() : base(54, 20, 80, -1, 62, 5, 9, 1.7f, 12f){}
	}
}

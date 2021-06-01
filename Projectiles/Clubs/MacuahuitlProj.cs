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
	class MacuahuitlProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Macuahuitl");
			Main.projFrames[projectile.type] = 3;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 120; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.MacuahuitlDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
		}
		public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.armorPenetration += (int)(projectile.ai[0] / 3);
        }
        public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int size = 66;
            if (projectile.ai[0] >= chargeTime)
            {

                Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, Truerotation, Origin, projectile.scale, Effects, 1);
            }
        }
        public MacuahuitlProj() : base(72, 24, 60, -1, 66, 5, 10, 1.9f, 17f){}
	}
}

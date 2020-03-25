using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class WardProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Ward");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 500;
			projectile.height = 130;
			projectile.width = 130;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y + 30);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)

            var list = Main.npc.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var npc in list)
            {
                if (!npc.friendly)
                {
                    npc.AddBuff(mod.BuffType("BCorrupt"), 20);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)projectile.height, (float)projectile.height) * projectile.scale * 1.45f / 2f;
                int index = Dust.NewDust(projectile.Center + vector2, 0, 0, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].position = projectile.Center + vector2;
                Main.dust[index].velocity = Vector2.Zero;
                Main.dust[index].noGravity = true;
            }
        }
	}
}

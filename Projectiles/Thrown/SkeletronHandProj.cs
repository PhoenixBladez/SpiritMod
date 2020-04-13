using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class SkeletronHandProj : ModProjectile
	{
		int timer = 0;
		// USE THIS DUST: 261
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Cutter");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 22;

			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;

			projectile.penetrate = 4;

			projectile.timeLeft = 160;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("SkeletronHand"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 37);
                Main.dust[d].scale *= .5f;
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool PreAI()
		{
            projectile.rotation += .3f;
			timer++;
			if (timer == 20 || timer == 40 || timer == 80)
				projectile.velocity *= 0.15f;
			else if (timer == 30 || timer == 90)
				projectile.velocity *= 10;
			else if (timer >= 100)
				timer = 0;

			return false;
		}

	}
}

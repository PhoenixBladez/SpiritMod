using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
			 ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.hostile = false;
			projectile.friendly = true;
            projectile.magic = true;
			projectile.alpha = 255;
            projectile.timeLeft = 140;
			projectile.penetrate = 2;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation += 0.05f;
             float num1 = 5f;
            float num2 = 3f;
            float num3 = 20f;
            num1 = 6f;
            num2 = 3.5f;
            if (projectile.timeLeft > 30 && projectile.alpha > 0)
                projectile.alpha -= 25;
            if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                projectile.alpha = 128;
            if (projectile.alpha < 0)
                projectile.alpha = 0;

            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                    projectile.frame = 0;
            }
            float num4 = 0.5f;
            if (projectile.timeLeft < 120)
                num4 = 1.1f;
            if (projectile.timeLeft < 60)
                num4 = 1.6f;

            ++projectile.ai[1];
            double num5 = (double)projectile.ai[1] / 180.0;
          
            
            int index1 = (int)projectile.ai[0];
            if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead)
            {
                if (projectile.Distance(Main.player[index1].Center) <= num3)
                    return;
                Vector2 unitY = projectile.DirectionTo(Main.player[index1].Center);
                if (unitY.HasNaNs())
                    unitY = Vector2.UnitY;
                projectile.velocity = (projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
            }
            else
            {
                if (projectile.timeLeft > 30)
                    projectile.timeLeft = 30;
                if (projectile.ai[0] == -1f)
                    return;
                projectile.ai[0] = -1f;
                projectile.netUpdate = true;
            }
            int num = 3;
		/*	for (int k = 0; k < 3; k++)
				{
					int index2 = Dust.NewDust(projectile.position, 1, 1, 226, 0.0f, 0.0f, 0, new Color(), .5f);
					Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
					Main.dust[index2].scale = .6f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;	
				}	*/

			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0f);
			}
		}
		
		 public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                //Vector2 drawPos1 = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY - 4);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                //Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_A1"), drawPos1, null, new Color (0, 50, 155, (int)((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)), 0f, drawOrigin, .6f, SpriteEffects.None, 0f);

            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 5;
			projectile.height = 5;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num621 = 0; num621 < 10; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;
                Main.dust[num622].noGravity = true;

            }
			for (int num623 = 0; num623 < 15; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}
	}
}
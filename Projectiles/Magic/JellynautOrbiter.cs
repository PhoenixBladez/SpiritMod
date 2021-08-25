using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
namespace SpiritMod.Projectiles.Magic
{
	public class JellynautOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Jellyfish");
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.penetrate = 2;
            projectile.hide = false;
			projectile.timeLeft = 9999;
		}
        float alphaCounter;
		public override void AI()
        {
            alphaCounter += .04f;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            projectile.frameCounter++;
            projectile.spriteDirection = -projectile.direction;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            float num1 = 10f;
            float num2 = 5f;
            float num3 = 40f;
            num1 = 10f;
            num2 = 7.5f;
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
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((projectile.Center.X + 10) - Main.screenPosition.X) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            int ypos = (int)((projectile.Center.Y + 10) - Main.screenPosition.Y) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), projectile.rotation, ripple.Size() / 2f, .5f * projectile.scale, spriteEffects, 0);
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 69);
            Vector2 direction = Main.MouseWorld - projectile.Center;
            direction.Normalize();
            direction *= 12f;
            {
                float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                int p = Projectile.NewProjectile(projectile.Center, direction,
                ModContent.ProjectileType<JellyfishOrbiter_Projectile>(), 16, 3, Main.myPlayer);
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
                Main.projectile[p].magic = true;
            }
        }
	}
}

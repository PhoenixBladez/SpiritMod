using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MoonburstProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonburst");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Valor);
			aiType = ProjectileID.Code1;
            projectile.width = projectile.height = 14;

        }
        float alphaCounter;
        public override void AI()
        {
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * .75f, 0.231f * .75f, 0.255f * .75f);
            alphaCounter += .04f;
            if (projectile.frameCounter >= 10)
            {
                projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.frameCounter++;
        }
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            if (projectile.frameCounter >= 1)
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 255, 255) * 0.95f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = (projectile.frameCounter * .13f) + .09f;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Yoyo/MoonburstBubble");
                    Texture2D tex1 = ModContent.GetTexture("SpiritMod/Projectiles/Yoyo/MoonburstBubble_Glow");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, 0f, tex.Size() / 2, scale, default, default);
                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * .4f, 0f, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                float sineAdd = (float)Math.Sin(alphaCounter) + 3;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (projectile.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
                Main.spriteBatch.Draw(ripple, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0) * .65f, projectile.rotation, ripple.Size() / 2f, projectile.frameCounter * .15f, spriteEffects, 0);
            }
            return true;
        }
		public override void Kill(int timeLeft)
        {
            ProjectileExtras.Explode(projectile.whoAmI, 150, 150, delegate
            {
                if (projectile.frameCounter >= 8)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 54));
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 118));
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].scale *= .25f;
                            if (Main.dust[num].position != projectile.Center)
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                    DustHelper.DrawDustImage(projectile.Center, 226, 0.29f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
                }
            });
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
    public class WizardBall : ModProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wizard's Moonjelly Ball");
        }
        public override void SetDefaults()
        {
            projectile.width = 68;
            projectile.height = 68;
            projectile.penetrate = -1; 
			projectile.friendly = false; 
            projectile.hostile = true; 
            projectile.aiStyle = -1; 
			projectile.scale = 0.8f;
			projectile.timeLeft = 110;
        }
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			projectile.Kill();
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f*2, 0.231f*2, 0.255f*2);
            if (projectile.timeLeft < 105 && projectile.timeLeft > 35)
            {
                projectile.velocity.Y += .06f;
            }
			if (projectile.timeLeft < 35)
            {
                projectile.velocity = Vector2.Zero;
                projectile.scale += .008f;
            }
			if (projectile.timeLeft % 10 == 0)
            {
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
  
                int p = Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-50, 50), projectile.Center.Y + Main.rand.Next(-50, 50), vector2_2.X, vector2_2.Y, ModContent.ProjectileType<WizardBallEnergyEffect>(), 0, 0.0f, Main.myPlayer, 0.0f, (float)projectile.whoAmI);
                Main.projectile[p].scale = Main.rand.NextFloat(.4f, 1.4f);

            }
        }
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < 1; k++)
                {
                    Color color = Color.White * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/MoonWizard/Projectiles/WizardBall_Glow");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 18; k++)
            {

                Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
                d.noGravity = true;
            }
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3.75f * 2f, 3.75f * 2f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -3.75f * 2f, -3.75f * 2f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);

            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 7.5f * 2f, 0f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -7.5f * 2f, 0f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 7.5f * 2f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -7.5f * 2f, mod.ProjectileType("WizardBall_Projectile"), projectile.damage, 0f, projectile.owner, 0f, 0f);

        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Color color1 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            int r1 = color1.R;
            drawOrigin.Y += 34f;
            drawOrigin.Y += 8f;
            --drawOrigin.X;
            Vector2 position1 = projectile.Center - Main.screenPosition;
            Texture2D texture2D2 = Main.glowMaskTexture[239];
            float num11 = (float)(Main.GlobalTime % 1.0 / 1.0);
            float num12 = num11;
            if (num12 > 0.5)
                num12 = 1f - num11;
            if (num12 < 0.0)
                num12 = 0.0f;
            float num13 = (float)((num11 + 0.5) % 1.0);
            float num14 = num13;
            if (num14 > 0.5)
                num14 = 1f - num13;
            if (num14 < 0.0)
                num14 = 0.0f;
            Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
            drawOrigin = r2.Size() / 2f;
            Vector2 position3 = position1 + new Vector2(6, 5);
            Color color3 = new Color(84, 207, 255) * 1.6f;
            Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, projectile.rotation, drawOrigin, projectile.scale * .73f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
            float num15 = 1f + num11 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, projectile.rotation, drawOrigin, projectile.scale * .73f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
            float num16 = 1f + num13 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, projectile.rotation, drawOrigin, projectile.scale * .73f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
            Texture2D texture2D3 = Main.extraTexture[89];
            Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
            drawOrigin = r3.Size() / 2f;
        }
    }
}

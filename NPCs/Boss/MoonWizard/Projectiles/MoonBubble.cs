using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonBubble : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((projectile.Center.X + 10) - Main.screenPosition.X) - (Main.projectileTexture[projectile.type].Width / 2);
            int ypos = (int)((projectile.Center.Y + 10) - Main.screenPosition.Y) - (Main.projectileTexture[projectile.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;

		float alphaCounter;
		public override void AI()
        {
            alphaCounter += 0.04f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f, 0.231f, 0.255f);
			if (projectile.timeLeft == 150) {
				projectile.scale *= Main.rand.NextFloat(0.6f, 1.1f);
			}
			if (projectile.ai[0] == 0) 
			{
				projectile.velocity.X *= 0.98f;
				projectile.velocity.Y -= 0.08f;
			}
			else
			{
				projectile.velocity *= 0.98f;
				projectile.timeLeft--;
			}
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 5;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .1825f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
			}
		}
	}
}

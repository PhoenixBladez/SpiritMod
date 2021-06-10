using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class AzureJelly : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Jelly");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Color color = Color.Pink;
			color.A = 0;
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((projectile.Center.X + 10) - Main.screenPosition.X) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            int ypos = (int)((projectile.Center.Y + 10) - Main.screenPosition.Y) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), color, projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        float alphaCounter;
		public override void AI()
        {
            alphaCounter += 0.04f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f, 0.231f, 0.255f);
			Player player = Main.player[(int)projectile.ai[0]];

			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			Vector2 direction = player.Center - projectile.Center;
			float rotDifference = ((((direction.ToRotation() - projectile.velocity.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

			projectile.velocity = projectile.velocity.RotatedBy(Math.Sign(rotDifference) * 0.01f);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 180, 0f, -2f, 0, default(Color), 2f);
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

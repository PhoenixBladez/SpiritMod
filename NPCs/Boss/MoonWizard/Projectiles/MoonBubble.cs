using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture2D1 = Main.projectileTexture[projectile.type];
			Vector2 position1 = projectile.Bottom - Main.screenPosition;
			Microsoft.Xna.Framework.Rectangle r1 = texture2D1.Frame(1, 1, 0, 0);
			Vector2 origin = r1.Size() / 2f;
			float num11 = (float)(Math.Cos((double)Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2 + 0.5);
			float num12 = num11;
			if ((double)num12 > 0.5)
				num12 = 1f - num11;
			if ((double)num12 < 0.0)
				num12 = 0.0f;
			float num13 = (float)(((double)num11 + 0.5) % 1.0);
			float num14 = num13;
			if ((double)num14 > 0.5)
				num14 = 1f - num13;
			if ((double)num14 < 0.0)
				num14 = 0.0f;
			float num16 = 1f + num13 * 0.75f;
			Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(75, 231, 255, 0) * 1.2f;
			Microsoft.Xna.Framework.Color color9 = new Microsoft.Xna.Framework.Color(75, 231, 255, 0) * 1.9f;
			Microsoft.Xna.Framework.Color color11 = new Microsoft.Xna.Framework.Color(75, 231, 255, 0) * 0.3f;
			Vector2 position3 = position1 + new Vector2(0.0f, 0f);
			Texture2D texture2D3 = mod.GetTexture("Effects/Ripple");
			Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
			origin = r3.Size() / 2f;
			Vector2 scale = new Vector2(0.75f, 1f + num16) * 0.65f * projectile.scale;
			Vector2 scale2 = new Vector2(1f + num16, 0.75f) * 0.65f * projectile.scale;
			float num17 = 1f + num13 * 0.75f;
			position3.Y -= 6f;
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, projectile.rotation + 1.570796f, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, projectile.rotation + 1.570796f, origin, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects1 = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int y2 = height * projectile.frame;
			Vector2 position = (projectile.position - (0.5f * projectile.velocity) + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
			float num1 = 1f;
			if (projectile.direction == 1) {
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Tiny_Moon_Jelly_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			else {
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Tiny_Moon_Jelly_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
			}
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f, 0.231f, 0.255f);
			if (projectile.timeLeft == 150) {
				projectile.scale = Main.rand.NextFloat(0.6f, 1.1f);
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
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .1825f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5f;
			}
		}
	}
}

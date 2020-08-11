using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class WizardBall_Jelly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Moonlight Jelly");
		}
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 0;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.timeLeft = 120;
			projectile.scale = 0.7f;
			Main.projFrames[projectile.type] = 5;
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
			float num11 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2 + 0.5);
			float num12 = num11;
			if ((double) num12 > 0.5)
			  num12 = 1f - num11;
			if ((double) num12 < 0.0)
			  num12 = 0.0f;
			float num13 = (float) (((double) num11 + 0.5) % 1.0);
			float num14 = num13;
			if ((double) num14 > 0.5)
			  num14 = 1f - num13;
			if ((double) num14 < 0.0)
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
			if (projectile.direction == 1)
			{
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Tiny_Moon_Jelly_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			else
			{		
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Tiny_Moon_Jelly_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, projectile.rotation, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
			}
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float) Math.Atan2((double) projectile.velocity.Y, (double) projectile.velocity.X) + 1.57f;
			
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 5;
			}
			
			float x = 0.15f;
			float y = 0.15f;

			Vector2 vector2_1 = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[(int)projectile.ai[0]].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[(int)projectile.ai[0]].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
			projectile.velocity = vector2_1;
			if ((double)projectile.velocity.Length() > 7f)
			{
				Vector2 vector2_2 = projectile.velocity * (6f / projectile.velocity.Length());
				projectile.velocity = vector2_2;
			}
			
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f, 0.231f, 0.255f);
		}
		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 1; ++index)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 4, 4, 226, 0.0f, 0.0f, 100, new Color(), 1f);
			}
			Player player = Main.player[Main.npc[(int)projectile.ai[0]].target];
			int a = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("WizardBall_Projectile"), 15, 6f, 0, 0.0f, 0.0f);
			Main.PlaySound(2, (int)Main.projectile[a].position.X, (int)Main.projectile[a].position.Y, 9, 1f, 0f);
			Vector2 vector2_1 = Main.projectile[a].velocity * 0.98f;
			Main.projectile[a].velocity = vector2_1;
			float num3 = 12.5f;
			Vector2 vector2_3 = new Vector2(Main.projectile[a].position.X + (float) Main.projectile[a].width * 0.5f, Main.projectile[a].position.Y + (float) Main.projectile[a].height * 0.5f);
			float num4 = player.position.X + (float) (player.width / 2) - vector2_3.X + Main.rand.Next(-180,180);
			float num5 = player.position.Y + (float) (player.height / 2) - vector2_3.Y + Main.rand.Next(-180,180);
			float num6 = (float) Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
			float num7 = num3 / num6;
			float num8 = num4 * num7;
			float num9 = num5 * num7;
			Main.projectile[a].velocity.X = num8;
			Main.projectile[a].velocity.Y = num9;
			Main.projectile[a].friendly = false;
			Main.projectile[a].hostile = true;
			float num = 8f;
			for (int index1 = 0; (double) index1 < (double) num; ++index1)
			{
				Vector2 v = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double) index1 * (6.28318548202515 / (double) num), new Vector2()) * new Vector2(1f, 4f)).RotatedBy((double) projectile.velocity.ToRotation(), new Vector2());
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 180, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].position = projectile.Center + v;
				Main.dust[index2].velocity = projectile.velocity * 0.0f + v.SafeNormalize(Vector2.UnitY) * 1f;
			}
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
    public class WizardBall : ModProjectile
    {
		public int randomCounter = 0;
		public int textureTimer = 0;
		Texture2D babyJelly;
        public override void SetDefaults()
        {
            projectile.width = 68;
            projectile.height = 68;
            projectile.penetrate = -1; 
			projectile.friendly = false; 
            projectile.hostile = true; 
            projectile.aiStyle = -1; 
			projectile.scale = 0.8f;
			projectile.timeLeft = 90;
        }
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wizard's Moonjelly Ball");
        }
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 28, 1f, 0f);
			int num = 20;
			for (int index1 = 0; index1 < num; ++index1)
			{
			  int index2 = Dust.NewDust(projectile.Center, 68, 68, 226, 0.0f, 0.0f, 0, new Color(), 0.75f);
			  Main.dust[index2].velocity *= 1.2f;
			  --Main.dust[index2].velocity.Y;
			  Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, projectile.Center, 0.75f);
			}
			for (int i = 0; i< (int)(projectile.ai[1]); i++)
			{
				int posX = Main.rand.Next(-50,50);
				int posY = Main.rand.Next(-50,50);
				int a = Projectile.NewProjectile(projectile.Center.X + posX, projectile.Center.Y + posY, 0f, 0f, mod.ProjectileType("WizardBall_Jelly"), 15, 3f, 0, 0.0f, 0.0f);
				Main.projectile[a].ai[0] = projectile.ai[0];
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 226, 0.0f, 0.0f, 0, new Color(), 0.3f);
				Main.dust[index2].velocity *= 1.2f;
				  --Main.dust[index2].velocity.Y;
				Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, new Vector2(projectile.Center.X + posX, projectile.Center.Y + posY), 0.75f);
			}
		}
		public override void AI()
		{
			if (Main.rand.Next(10)==0)
			{
				int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 100, new Color(), 0.8f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity.X /= 2f;
				Main.dust[index].velocity.Y /= 2f;
			}
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f*2, 0.231f*2, 0.255f*2);
			if (projectile.velocity.Y != 0f)
				projectile.velocity.Y += 0.08f;
			if (projectile.timeLeft == 90-30)
			{
				Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 28, 1f, 0f);
				int num = 20;
				for (int index1 = 0; index1 < num; ++index1)
				{
				  int index2 = Dust.NewDust(projectile.Center, 68, 68, 226, 0.0f, 0.0f, 0, new Color(), 0.75f);
				  Main.dust[index2].velocity *= 1.2f;
				  --Main.dust[index2].velocity.Y;
				  Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, projectile.Center, 0.75f);
				}
				int direction = 0;
				if (Main.npc[(int)projectile.ai[0]].position.X > projectile.position.X)
					direction = -1;
				else
					direction = 1;
				projectile.velocity.X = direction * 6f;
				projectile.velocity.Y = -4f;
			}		
			
			
			randomCounter++;
			float num4 = 2.094395f;
			float f1 = (float) ((double) projectile.localAI[0] % 6.28318548202515 - 3.14159274101257);
			float num11 = (float) Math.IEEERemainder((double) projectile.localAI[1], 1.0);
			if ((double) num11 < 0.0)
				++num11;
			float num12 = (float) Math.Floor((double) projectile.localAI[1]);
			float max = 0.999f;
			int num13 = 0;
			float amount = 0.1f;
			float f2 = 0f;
			float num16;		
			float num17;
			f2 = projectile.AngleTo(projectile.Center + ((float) ((double) (float) randomCounter / 120f * 6.28318548202515 + (double) num4)).ToRotationVector2() * projectile.height);
			num13 = 3; 
			num16 = MathHelper.Clamp(num11 + 0.05f, 0.0f, max);
			num17 = num12 + (float) Math.Sign(6f - num12);

			Vector2 rotationVector2 = f2.ToRotationVector2();
			projectile.localAI[0] = (float) ((double) Vector2.Lerp(f1.ToRotationVector2(), rotationVector2, amount).ToRotation() + (double) num13 * 6.28318548202515 + 3.14159274101257);
			projectile.localAI[1] = num17 + num16;
		}
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
			Vector2 position1 = new Vector2(projectile.position.X, projectile.position.Y-8) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Vector2 position5 = new Vector2(projectile.position.X-10, projectile.position.Y-8) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Vector2 position6 = new Vector2(projectile.position.X+15, projectile.position.Y-8) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Vector2 position2 = new Vector2(projectile.position.X, projectile.position.Y-18) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Vector2 position3 = new Vector2(projectile.position.X + 15, projectile.position.Y +2) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Vector2 position4 = new Vector2(projectile.position.X - 8, projectile.position.Y+12) + new Vector2((float)projectile.width, (float)projectile.height + 4) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Microsoft.Xna.Framework.Color alpha = projectile.GetAlpha(lightColor);
			int num1 = (int) ((double) projectile.localAI[0] / 6.28318548202515);
            float f = (float) ((double) projectile.localAI[0] % 6.28318548202515 - 3.14159274101257);
            float num2 = (float) Math.IEEERemainder((double) projectile.localAI[1], 1.0);
            if ((double) num2 < 0.0)
              ++num2;
            int num3 = (int) Math.Floor((double) projectile.localAI[1]);
            float num4 = 5f;          
            if ((double) num1 == 1.0)
              num4 = 14f;
			float num44 = 9f;          
            if ((double) num1 == 1.0)
              num44 = 19f;
            Vector2 vector2 = -f.ToRotationVector2() * num2 * num4 * projectile.scale;
            Vector2 vector22 = f.ToRotationVector2() * num2 * num4 * projectile.scale;
			Vector2 vector3 = -f.ToRotationVector2() * num2 * num44 * projectile.scale;
            Vector2 vector33 = f.ToRotationVector2() * num2 * num44 * projectile.scale;
			textureTimer++;
			if (textureTimer < 7)
			{
				babyJelly = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1");
			}
			else if (textureTimer < 14)
			{
				babyJelly = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_2");
			}
			else if (textureTimer < 21)
			{
				babyJelly = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_3");
			}
			else if (textureTimer < 28)
			{
				babyJelly = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_4");
			}
			else if (textureTimer < 35)
			{
				babyJelly = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_5");
			}
			else
			{
				textureTimer = 0;
			}
			
			Main.spriteBatch.Draw(babyJelly, position1+vector2, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.4f, SpriteEffects.None, 0.0f);
			Main.spriteBatch.Draw(babyJelly, position2+vector22, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.5f, SpriteEffects.None, 0.0f);
			Main.spriteBatch.Draw(babyJelly, position3+vector33, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.6f, SpriteEffects.None, 0.0f);
			Main.spriteBatch.Draw(babyJelly, position4+vector3, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.45f, SpriteEffects.None, 0.0f);
			Main.spriteBatch.Draw(babyJelly, position5+vector22, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.8f, SpriteEffects.None, 0.0f);
			Main.spriteBatch.Draw(babyJelly, position6+vector3, new Microsoft.Xna.Framework.Rectangle?(), Color.White, projectile.rotation, babyJelly.Size() / 2f, 0.35f, SpriteEffects.None, 0.0f);
            return false;
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 vector2_3 = new Vector2((float) (Main.projectileTexture[projectile.type].Width / 2), (float) (Main.projectileTexture[projectile.type].Height / 1 / 2));
			int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, height * projectile.frame, texture.Width, height);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), drawColor, projectile.rotation, vector2_3, projectile.scale, spriteEffects, 0);
			float num9 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2.0 + 0.5);
			Vector2 bb = projectile.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * projectile.scale / 2f + vector2_3 * projectile.scale + new Vector2(0.0f, projectile.gfxOffY);
			Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - projectile.alpha, (int) sbyte.MaxValue - projectile.alpha, (int) sbyte.MaxValue - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
			Texture2D texture2D1 = Main.projectileTexture[projectile.type];
			Microsoft.Xna.Framework.Rectangle r1 = texture2D1.Frame(1, 1, 0, 0);
			Vector2 origin = r1.Size() / 2f;
			Vector2 position1 = projectile.Bottom - Main.screenPosition;
			Texture2D glow = mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/WizardBall_Glow");
			float num11 = (float) ((double) Main.GlobalTime % 5.0 / 5.0);
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
			Texture2D texture2D2 = Main.glowMaskTexture[239];
			Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
			origin = r2.Size() / 2f;
			Vector2 position3 = position1 + new Vector2(0f, -28f);
			Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(75, 231, 255, 0) * 0.5f;
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, projectile.rotation, origin, projectile.scale * 0.73f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, projectile.rotation, origin, projectile.scale * 0.73f * num15, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, projectile.rotation, origin, projectile.scale * 0.73f * num16, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			Texture2D texture2D3 = mod.GetTexture("Effects/Ripple");
			Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
			origin = r3.Size() / 2f;
			Vector2 scale = new Vector2(0.75f, 1f + num16) * 2f;
			float num17 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, projectile.rotation + 1.570796f, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), Microsoft.Xna.Framework.Color.Lerp(color3, Microsoft.Xna.Framework.Color.White, 0.5f), projectile.rotation + 1.570796f, origin, projectile.scale * 2f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(glow, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), color2, projectile.rotation, vector2_3, projectile.scale, spriteEffects, 0.0f);
		}
    }
}

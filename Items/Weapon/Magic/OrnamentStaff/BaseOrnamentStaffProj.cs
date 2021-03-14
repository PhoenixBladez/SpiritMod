using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.OrnamentStaff
{
	public abstract class BaseOrnamentStaffProj : ModProjectile
	{
		internal bool homing = false;
		internal bool speedCheck = false;
		internal float speed = 0f;
		public int DustType;
		public Color color;

		public BaseOrnamentStaffProj(int DustType, Color color)
		{
			this.DustType = DustType;
			this.color = color;
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 170;
			projectile.netUpdate = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.570796f;
			Lighting.AddLight(projectile.Center, color.ToVector3() / 2);
			if (projectile.timeLeft < 120)
			{
				if (!homing && projectile.owner == Main.myPlayer)
				{
					projectile.ai[0] = Main.MouseWorld.X;
					projectile.ai[1] = Main.MouseWorld.Y;
					Vector2 vector2_1 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
					Vector2 vector2_2 = Vector2.Normalize(vector2_1 - projectile.Center) * 14f;
					projectile.velocity = vector2_2;
					homing = true;
					projectile.netUpdate = true;
				}

				if (Vector2.Distance(projectile.Center, new Vector2(projectile.ai[0], projectile.ai[1])) <= 12.0) 
					projectile.timeLeft = Math.Min(projectile.timeLeft, 5);
				
			}
			else
			{
				if (!speedCheck)
				{
					speed = Main.rand.NextFloat(0.9f, 0.95f);
					speedCheck = true;
				}
				projectile.velocity *= speed;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 4);

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) //hell
		{
			SpriteEffects effects1 = SpriteEffects.None;
			if (projectile.direction == 1)
				effects1 = SpriteEffects.FlipHorizontally;
			Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int) ((double) projectile.position.X + (double) projectile.width * 0.5) / 16, (int) (((double) projectile.position.Y + (double) projectile.height * 0.5) / 16.0));
			{
				Texture2D texture = Main.projectileTexture[projectile.type];
				Texture2D glow = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, height * projectile.frame, texture.Width, height);
				Vector2 origin = r.Size() / 2f;
				int num2 = 5;
				int num3 = 1;
				int num4 = 1;
				float num5 = 1f;
				float num6 = 0.0f;
				num3 = 1;
				num5 = 3f;
				int index1 = num4;
				while (num3 > 0 && index1 < num2 || num3 < 0 && index1 > num2)
				{
				  Microsoft.Xna.Framework.Color newColor = color3;
				  newColor = Microsoft.Xna.Framework.Color.Lerp(newColor, Microsoft.Xna.Framework.Color.White, 2.5f);
				  Microsoft.Xna.Framework.Color color1 = projectile.GetAlpha(newColor);
				  float num7 = (float) (num2 - index1);
				  if (num3 < 0)
					num7 = (float) (num4 - index1);
				  Microsoft.Xna.Framework.Color color2 = color1 * (num7 / ((float) ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f));
				  Vector2 oldPo = projectile.oldPos[index1];
				  float rotation = projectile.rotation;
				  SpriteEffects effects2 = effects1;
				  if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				  {
					rotation = projectile.oldRot[index1];
					effects2 = projectile.oldSpriteDirection[index1] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				  }
				  Main.spriteBatch.Draw(glow, oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), color2, rotation + projectile.rotation * num6 * (float) (index1 - 1) * (float) -effects1.HasFlag((Enum) SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale, num5, (float) index1 / 15f), effects2, 0.0f);
				label_709:
				  index1 += num3;
				}

				Microsoft.Xna.Framework.Color color4 = projectile.GetAlpha(color3);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), new Color(255-projectile.alpha, 255-projectile.alpha, 255-projectile.alpha, 175), projectile.rotation, origin, projectile.scale, effects1, 0.0f);
			}
			for (int a = 0; a<1; a++)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (projectile.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.projectileTexture[projectile.type];
				Vector2 vector2_3 = new Vector2((float) (Main.projectileTexture[projectile.type].Width / 2), (float) (Main.projectileTexture[projectile.type].Height / 1 / 2));
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, height * projectile.frame, texture.Width, height);
				float addY = 0f;
				float addHeight = 0f;		
				int num7 = 5;
				float num9 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2.0 + 0.5);
				float num99 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
				float num8 = 0f;
				Microsoft.Xna.Framework.Color secondColor = Microsoft.Xna.Framework.Color.White;
				
				float num10 = 0.0f;
				Vector2 bb = projectile.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * projectile.scale / 2f + vector2_3 * projectile.scale + new Vector2(0.0f, addY + addHeight + projectile.gfxOffY);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - projectile.alpha, (int) sbyte.MaxValue - projectile.alpha, (int) sbyte.MaxValue - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
				for (int index2 = 0; index2 < 4; ++index2)
				{
					Microsoft.Xna.Framework.Color newColor2 = color2;
					Microsoft.Xna.Framework.Color faa = projectile.GetAlpha(newColor2) * (1f - num99);
					Vector2 position2 = projectile.Center + ((float) ((double) index2 / (double) 4 * 6.28318548202515) + projectile.rotation + num10).ToRotationVector2() * (float) (8.0 * (double) num99 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * projectile.scale / 2f + vector2_3 * projectile.scale + new Vector2(0.0f, addY + addHeight + projectile.gfxOffY);
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(r), faa, projectile.rotation, vector2_3, projectile.scale, spriteEffects, 0.0f);
				}
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 spinningpoint = new Vector2(0f, -3f).RotatedByRandom(3.14159274101257);
			float num1 = (float) 18*projectile.scale;
			Vector2 vector2 = new Vector2(1.1f, 1f);
			for (float num2 = 0.0f; (double) num2 < (double) num1; ++num2)
			{
				int dustIndex = Dust.NewDust(projectile.Center, 0, 0, DustType, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[dustIndex].position = projectile.Center;
				Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double) num2 / (double) num1, new Vector2()) * vector2 * (float) (0.800000011920929 + (double) Main.rand.NextFloat() * 0.400000005960464);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				Dust dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;	  
			}

			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 165, 0.5f, 0.0f);
		}
	}
}

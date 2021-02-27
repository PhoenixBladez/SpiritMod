using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow.OrnamentBow
{
	public class Ornament_Arrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornament Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.arrow = true;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 4 && targetHitbox.Height > 4)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 4, -targetHitbox.Height / 4);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		public override void AI()
		{
			for (int i = 0; i < 1; i++)
			{
				int num;
				num = projectile.frameCounter;
				projectile.frameCounter = num + 1;
				projectile.localAI[0] += 1f;
				Vector2 value7 = new Vector2(2f, 10f);
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = Vector2.UnitX * 30f;
					value8 = -Vector2.UnitY.RotatedBy((double)(projectile.localAI[0] * 0.1308997f + (float)num41 * 3.14159274f), default(Vector2)) * value7 - projectile.rotation.ToRotationVector2();
					int num42 = Dust.NewDust(projectile.Center, 0, 0, 66, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
					Main.dust[num42].scale = 0.7f;
					Main.dust[num42].noGravity = true;
					Main.dust[num42].position = projectile.Center + value8 + projectile.velocity * 2f;
					Main.dust[num42].velocity = Vector2.Normalize(projectile.Center + projectile.velocity * 2f - Main.dust[num42].position) / 8f + projectile.velocity;
					num = num41;
				}
			}
			for (int i = 0; i < 1; i++)
			{
				int num;
				num = projectile.frameCounter;
				projectile.frameCounter = num + 1;
				projectile.localAI[0] += 1f;
				Vector2 value7 = new Vector2(10f, 2f);
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = Vector2.UnitX * 30f;
					value8 = -Vector2.UnitY.RotatedBy((double)(projectile.localAI[0] * 0.1308997f + (float)num41 * 3.14159274f), default(Vector2)) * value7 - projectile.rotation.ToRotationVector2();
					int num42 = Dust.NewDust(projectile.Center, 0, 0, 66, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
					Main.dust[num42].scale = 0.7f;
					Main.dust[num42].noGravity = true;
					Main.dust[num42].position = projectile.Center + value8 + projectile.velocity * 2f;
					Main.dust[num42].velocity = Vector2.Normalize(projectile.Center + projectile.velocity * 2f - Main.dust[num42].position) / 8f + projectile.velocity;
					num = num41;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
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
			for (int index = 0; index < 10; ++index)
			{
				int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 66, 0.0f, 0.0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
				Main.dust[i].noGravity = true;
			}
			Main.PlaySound(42, (int)projectile.position.X, (int)projectile.position.Y, 193, 1f, -0.2f);
			Player player = Main.player[projectile.owner];
			for (int i = 0; i < 1 + Main.rand.Next(5); i++)
			{
				int arrowType = Main.rand.Next(6);
				int dustType = 1;
				switch (arrowType)
				{
					case 0:
						arrowType = mod.ProjectileType("Amethyst_Arrow");
						dustType = 86;
						break;
					case 1:
						arrowType = mod.ProjectileType("Topaz_Arrow");
						dustType = 87;
						break;
					case 2:
						arrowType = mod.ProjectileType("Sapphire_Arrow");
						dustType = 88;
						break;
					case 3:
						arrowType = mod.ProjectileType("Emerald_Arrow");
						dustType = 89;
						break;
					case 4:
						arrowType = mod.ProjectileType("Ruby_Arrow");
						dustType = 90;
						break;
					case 5:
						arrowType = mod.ProjectileType("Diamond_Arrow");
						dustType = 91;
						break;
					default:
						break;
				}
				float positionX = Main.rand.Next(-80,80);
				float positionY = Main.rand.Next(-60,-20);
				int a = Projectile.NewProjectile(player.Center.X + positionX, player.Center.Y + positionY, 0f, 0f, arrowType, player.HeldItem.damage/2, 2f, player.whoAmI);
				Vector2 vector2_2 = Vector2.Normalize(new Vector2(projectile.Center.X, projectile.Center.Y) - Main.projectile[a].Center) * (float) Main.rand.Next(12,14);
				Main.projectile[a].velocity = vector2_2;
				float nigga = 16f;
				for (int index1 = 0; (double)index1 < (double)nigga; ++index1)
				{
					Vector2 v = (Vector2.UnitX * 0.1f + -Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)nigga), new Vector2()) * new Vector2(4f, 4f)).RotatedBy((double)new Vector2(projectile.velocity.X, projectile.velocity.Y).ToRotation(), new Vector2());
					int index2 = Dust.NewDust(new Vector2(player.Center.X + positionX, player.Center.Y + positionY), 8, 8, dustType, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].scale = 0.9f;
					Main.dust[index2].alpha = 200;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
					Main.dust[index2].position = new Vector2(player.Center.X + positionX, player.Center.Y + positionY) + v;
					Main.dust[index2].velocity = new Vector2(projectile.velocity.X, projectile.velocity.Y) * 0.0f + v.SafeNormalize(Vector2.UnitY) * 1f;
				}
			}
		}
	}
}

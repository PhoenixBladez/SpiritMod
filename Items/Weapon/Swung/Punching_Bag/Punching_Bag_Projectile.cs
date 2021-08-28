using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Punching_Bag
{
	public class Punching_Bag_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie's Punch");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 60;
			projectile.alpha = 100;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effect = projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(projectile.Center.Y) / 16, (int)(projectile.Center.Y) / 16);
			var basePos = projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY);

			Texture2D texture = Main.projectileTexture[projectile.type];

			int height = texture.Height / Main.projFrames[projectile.type];
			var frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f));
				Vector2 oldPo = projectile.oldPos[reps];
				float rotation = projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				{
					rotation = projectile.oldRot[reps];
					effects2 = projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 175), projectile.rotation, origin, projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[projectile.type];
			frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + projectile.rotation).ToRotationVector2();
				Vector2 position2 = projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * projectile.scale / 2f + origin * projectile.scale + new Vector2(0.0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, projectile.rotation, origin, projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		public override void AI()
		{
			projectile.alpha += 10;
			projectile.velocity*=0.97f;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.BubbleBurst_Green, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, DustID.BubbleBurst_Green, projectile.oldVelocity.X, projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, DustID.BubbleBurst_Green, projectile.oldVelocity.X, projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.BubbleBurst_Green, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Punching_Bag
{
	public class Punching_Bag_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie's Punch");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 16;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 60;
			Projectile.alpha = 100;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effect = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(Projectile.Center.Y) / 16, (int)(Projectile.Center.Y) / 16);
			var basePos = Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY);

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			int height = texture.Height / Main.projFrames[Projectile.type];
			var frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = Projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f));
				Vector2 oldPo = Projectile.oldPos[reps];
				float rotation = Projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[Projectile.type] == 2)
				{
					rotation = Projectile.oldRot[reps];
					effects2 = Projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + Projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(Projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 175), Projectile.rotation, origin, Projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[Projectile.type];
			frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = Projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + Projectile.rotation).ToRotationVector2();
				Vector2 position2 = Projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * Projectile.scale / 2f + origin * Projectile.scale + new Vector2(0.0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, Projectile.rotation, origin, Projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		public override void AI()
		{
			Projectile.alpha += 10;
			Projectile.velocity*=0.97f;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.BubbleBurst_Green, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = Projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = Projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num1, Projectile.oldPosition.Y - num2), 8, 8, DustID.BubbleBurst_Green, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = Projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = Projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num1, Projectile.oldPosition.Y - num2), 8, 8, DustID.BubbleBurst_Green, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.BubbleBurst_Green, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
	}
}

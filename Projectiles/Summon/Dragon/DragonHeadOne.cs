using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Dragon
{
	public class DragonHeadOne : ModProjectile
	{
		int counter = -180;
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 6;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 95;
			Projectile.DamageType = DamageClass.Magic;
			//projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 32;

		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(66 - (int)(num / 3 * 2), 245 - (int)(num / 3 * 2), 120 - (int)(num / 3 * 2), 255 - num);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		int num;
		public override void AI()
		{
			num += 4;
			Projectile.alpha += 12;
			Projectile.spriteDirection = 1;
			if (Projectile.ai[0] > 0) {
				Projectile.spriteDirection = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			distance += 0.03f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			Projectile.velocity = initialSpeed + offset;
		}
	}
}
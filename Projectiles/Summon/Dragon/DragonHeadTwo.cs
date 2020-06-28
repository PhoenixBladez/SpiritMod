using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Dragon
{
	public class DragonHeadTwo : ModProjectile
	{
		int counter = 0;
		float distance = 4;
		int rotationalSpeed = 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 6;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 190;
			projectile.damage = 13;
			projectile.minionSlots = 0.5f;
			projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		int num;
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(66 - (int)(num / 3 * 2), 245 - (int)(num / 3 * 2), 120 - (int)(num / 3 * 2), 255 - num);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		public override void AI()
		{
			num += 2;
			projectile.alpha += 6;
			projectile.spriteDirection = 1;
			if(projectile.ai[0] > 0) {
				projectile.spriteDirection = 0;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.015f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
			bool flag25 = false;
			int jim = 1;
			for(int index1 = 0; index1 < 200; index1++) {
				if(Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
					if(num25 < 500f) {
						flag25 = true;
						jim = index1;
					}

				}
			}
			if(flag25) {
				float num1 = 12.5f;
				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				Vector2 direction5 = Main.npc[jim].Center - projectile.Center;
				direction5.Normalize();
				projectile.rotation = projectile.DirectionTo(Main.npc[jim].Center).ToRotation() + 1.57f;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				if(Main.rand.Next(16) == 0) {
					projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
					projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
				}
			}
		}
	}
}
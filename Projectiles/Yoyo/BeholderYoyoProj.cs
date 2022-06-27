using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class BeholderYoyoProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of the Beholder");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Valor);
			AIType = ProjectileID.Valor;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;
		}
		int manaTimer;
		public override void AI()
		{
			for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.PurificationPowder);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].scale *= .6f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 24f;
				Main.dust[dust].position = Projectile.Center - vector2_3;
			}
			if (Main.myPlayer == Projectile.owner) {
				if (Main.player[Projectile.owner].channel && Main.player[Projectile.owner].statMana > 0) {
					manaTimer++;
					if (manaTimer >= 4) {
						manaTimer = 0;
						Main.player[Projectile.owner].statMana--;
					}
				}
				if (Main.player[Projectile.owner].statMana <= 0) {
					Projectile.Kill();
				}
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 180) {
				Projectile.frameCounter = 0;
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) {
					float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(Projectile, false)) {
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1) {
					bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) {
						Vector2 value = Main.npc[num2].Center - Projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4) {
							num5 = num4 / num5;
						}
						value *= num5;
						int p = Terraria.Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ProjectileID.Fireball, Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
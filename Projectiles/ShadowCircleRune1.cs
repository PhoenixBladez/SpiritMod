using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowCircleRune1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Circle Rune");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 86;
			Projectile.height = 80;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 360;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			Projectile.rotation += 1f * (float)Projectile.direction;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)
			Projectile.rotation += player.direction * 0.5f;
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] >= 10f) {
					Projectile.localAI[0] = 0f;
					int num416 = 0;
					int num417 = 0;
					float num418 = 0f;
					int num419 = Projectile.type;
					for (int num420 = 0; num420 < 1000; num420++) {
						if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
							num416++;
							if (Main.projectile[num420].ai[1] > num418) {
								num417 = num420;
								num418 = Main.projectile[num420].ai[1];
							}
						}
					}

					if (num416 > 2) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 40; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.NextBool(2)) {
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

	}
}

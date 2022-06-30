using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PalladiumRuneEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Rune");
			Main.projFrames[Projectile.type] = 10;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.timeLeft = 60;

		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(252 - (int)(timer / 3), 153 - (int)(timer / 3), 3 - (int)(timer / 3), 255 - timer * 2);
		}
		int timer;
		public override void AI()
		{
			Lighting.AddLight(Projectile.position, 0.4f / 2, .12f / 2, .036f / 2);
			timer += 4;
			Projectile.alpha += 6;
			Projectile.velocity.X *= .98f;
			Projectile.velocity.Y *= .98f;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 10) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 10;
			}
		}
		public override void Kill(int timeLeft)
		{
			{
				for (int i = 0; i < 10; i++) {
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, -2f, 0, Color.White, 2f);
					Main.dust[num].noLight = true;
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
					if (Main.dust[num].position != Projectile.Center) {
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
					}
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frameRect, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class SpaceDebris4 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Junk");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.height = 20;
			projectile.width = 18;
			aiType = ProjectileID.DeathLaser;
			projectile.extraUpdates = 1;
		}

		int timer = 1;
		public override void AI()
		{
			projectile.localAI[0] += 1f;
			if(projectile.localAI[0] == 16f) {
				projectile.localAI[0] = 0f;
				for(int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 6, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			int num = 5;
			for(int k = 0; k < 4; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 6, 0.0f, 0.0f, 0, new Color(), .9f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int num625 = 0; num625 < 2; num625++) {
				float scaleFactor10 = 0.33f;
				if(num625 == 1)
					scaleFactor10 = 0.66f;

				if(num625 == 2)
					scaleFactor10 = 1f;

				int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13AB6_cp_0 = Main.gore[num626];
				expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
				Gore expr_13AD6_cp_0 = Main.gore[num626];
				expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13B79_cp_0 = Main.gore[num626];
				expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
				Gore expr_13B99_cp_0 = Main.gore[num626];
				expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for(int i = 0; i < 4; i++) {
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num624].velocity *= 0f;
				Main.dust[num624].scale *= 0.3f;
			}
			if(Main.rand.Next(0, 20) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ModContent.ItemType<SpaceJunkItem>(), 1, false, 0, false, false);
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
	}
}

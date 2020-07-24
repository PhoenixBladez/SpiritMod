using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class MarbleLongbowArrow : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 6;
			projectile.height = 12;
			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		public override void AI()
		{
			for(int k = 0; k < 3; k++) {
				Dust d = Dust.NewDustPerfect(projectile.Center, 222, Vector2.Normalize(projectile.velocity) * -k, 0, default, 0.3f);
				d.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();

		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.DD2_LightningBugHurt, (int)projectile.position.X, (int)projectile.position.Y);

			for(int k = 0; k < 25; k++) {
				Dust d = Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.5f);
				d.noGravity = true;
			}

			int num2 = Main.rand.Next(2, 4);
			int num3 = Main.rand.Next(0, 360);
			int num24 = ModContent.ProjectileType<MarbleArrowStone>();
			for(int j = 0; j < num2; j++) {
				float num4 = MathHelper.ToRadians((float)(270 / num2 * j + num3));
				Vector2 vector = new Vector2(base.projectile.velocity.X, base.projectile.velocity.Y).RotatedBy((double)num4, default(Vector2));
				vector.Normalize();
				vector.X *= 3.5f;
				vector.Y *= 3.5f;
				int p = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, num24, projectile.damage / 5 * 2, 0f, 0);
				Main.projectile[p].hostile = false;
				Main.projectile[p].friendly = true;
			}
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale * (float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length * 0.2f;
				Texture2D tex = GetTexture("SpiritMod/Textures/Glow");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, 0, tex.Size() / 2, scale, default, default);
				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * 0.3f, 0, tex.Size() / 2, scale * 4, default, default);
			}
		}
	}
}

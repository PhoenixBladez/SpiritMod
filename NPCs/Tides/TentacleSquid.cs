using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class TentacleSquid : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'ylheian");
			Main.projFrames[base.projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 40;

			projectile.damage = 3;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - 3.14f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6) {
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			projectile.scale = num395 + 0.95f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 3.14f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.Butterfly, 0f, 0f, 0, default, 1.6f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

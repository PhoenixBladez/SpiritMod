using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class SadSoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Sadness");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		int timer = 25;

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.sentry = true;
			Projectile.width = 24;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
		}
		public override void AI()
		{
			timer--;
			Projectile.alpha += 5;
			if (Projectile.alpha >= 200) {
				Projectile.alpha = 200;
			}
			if (timer <= 0) {
				Projectile.NewProjectile(Projectile.Center.X + 5, Projectile.Center.Y + 7, 0, Main.rand.Next(8, 18), ModContent.ProjectileType<SadBeam>(), 13, Projectile.knockBack, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.Center.X - 5, Projectile.Center.Y + 7, 0, Main.rand.Next(8, 18), ModContent.ProjectileType<SadBeam>(), 13, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 40;
			}

			if (Projectile.localAI[0] == 0f) {
				Projectile.localAI[0] = Projectile.Center.Y;
				Projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (Projectile.Center.Y >= Projectile.localAI[0]) {
				Projectile.localAI[1] = -1f;
				Projectile.netUpdate = true;
			}
			if (Projectile.Center.Y <= Projectile.localAI[0] - 2f) {
				Projectile.localAI[1] = 1f;
				Projectile.netUpdate = true;
			}
			Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y + 0.009f * Projectile.localAI[1], -.75f, .75f);

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
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(252, 252, 252, Projectile.alpha);
		}
	}
}

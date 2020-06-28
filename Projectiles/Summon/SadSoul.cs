using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class SadSoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Sadness");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 13;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		int timer = 25;

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 24;
			projectile.timeLeft = 3600;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 24;
			projectile.aiStyle = -1;
		}
		public override void AI()
		{
			timer--;
			projectile.alpha += 5;
			if(projectile.alpha >= 200) {
				projectile.alpha = 200;
			}
			if(timer <= 0) {
				Projectile.NewProjectile(projectile.Center.X + 5, projectile.Center.Y + 7, 0, Main.rand.Next(8, 18), ModContent.ProjectileType<SadBeam>(), 13, projectile.knockBack, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X - 5, projectile.Center.Y + 7, 0, Main.rand.Next(8, 18), ModContent.ProjectileType<SadBeam>(), 13, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 40;
			}

			if(projectile.localAI[0] == 0f) {
				projectile.localAI[0] = projectile.Center.Y;
				projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if(projectile.Center.Y >= projectile.localAI[0]) {
				projectile.localAI[1] = -1f;
				projectile.netUpdate = true;
			}
			if(projectile.Center.Y <= projectile.localAI[0] - 2f) {
				projectile.localAI[1] = 1f;
				projectile.netUpdate = true;
			}
			projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.009f * projectile.localAI[1], -.75f, .75f);

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
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(252, 252, 252, projectile.alpha);
		}
	}
}

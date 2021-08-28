using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock1 : ModProjectile, ITrailProjectile

    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.alpha = 255;
			projectile.timeLeft = 140;
			projectile.penetrate = 2;
			projectile.extraUpdates = 1;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 250f, new DefaultShader());
		}

		public override void AI()
		{
			float num1 = 5f;
			float num2 = 3f;
			float num3 = 20f;
			num1 = 6f;
			num2 = 3.5f;
			if (projectile.timeLeft > 30 && projectile.alpha > 0)
				projectile.alpha -= 25;
			if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				projectile.alpha = 128;
			if (projectile.alpha < 0)
				projectile.alpha = 0;

			if (++projectile.frameCounter > 4) {
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
					projectile.frame = 0;
			}

			++projectile.ai[1];
			double num5 = (double)projectile.ai[1] / 180.0;


			if (Main.player[projectile.owner].active && !Main.player[projectile.owner].dead) {
				if (projectile.Distance(Main.player[projectile.owner].Center) <= num3)
					return;
				Vector2 unitY = projectile.DirectionTo(Main.player[projectile.owner].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				projectile.velocity = (projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
			}
			else {
				if (projectile.timeLeft > 30)
					projectile.timeLeft = 30;
				if (projectile.ai[0] == -1f)
					return;
				projectile.ai[0] = -1f;
				projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				//Vector2 drawPos1 = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY - 4);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				//Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_A1"), drawPos1, null, new Color (0, 50, 155, (int)((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)), 0f, drawOrigin, .6f, SpriteEffects.None, 0f);

			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void Kill(int timeLeft)
		{
			int z = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Main.projectile[z].magic = true;
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 5;
			projectile.height = 5;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num621 = 0; num621 < 10; num621++) {
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, 1f);
				Main.dust[num622].velocity *= 1f;
				Main.dust[num622].noGravity = true;

			}
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}
	}
}
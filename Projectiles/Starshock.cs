using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class Starshock : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starshock");

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			projectile.tileCollide = false;
			projectile.timeLeft = 100;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 250f, new DefaultShader());
		}

		public override void AI()
		{
			if (++projectile.ai[0] > 20f) {
				bool canaccelerate = projectile.velocity.Length() < 18;
				projectile.tileCollide = !canaccelerate;
				if (canaccelerate)
					projectile.velocity = projectile.velocity * 1.02f;
			}

			projectile.rotation += 0.1f;

			if (projectile.localAI[1] == 0f) {
				projectile.localAI[1] = 1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.DungeonSpirit, projectile.oldVelocity.X * 0.1f, projectile.oldVelocity.Y * 0.1f);
				Main.dust[d].noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, Main.projectileTexture[projectile.type].Bounds, projectile.GetAlpha(Color.White), projectile.rotation, 
				Main.projectileTexture[projectile.type].Size()/2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
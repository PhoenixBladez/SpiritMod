using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenfleetStar : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			DisplayName.SetDefault("Heaven Fleet");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			projectile.tileCollide = true;
			projectile.timeLeft = 150;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 13f, 150f, new DefaultShader());
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .35f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 22f, 150f, new DefaultShader());
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .125f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 34f, 250f, new DefaultShader());
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .1f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 46f, 250f, new DefaultShader());
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245) * .24f, new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 18f, 220f, new DefaultShader());
		}

		public override void AI()
		{
			projectile.rotation += .05f*projectile.velocity.X;
			projectile.ai[0] += .0205f;
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(projectile.Center, 6, 6, DustID.FireworkFountain_Blue, 0f, 0f, 0, default, .65f);
				Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;

				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(27, Main.LocalPlayer);
				Main.dust[num].velocity = projectile.velocity;
				Main.dust[num].scale = MathHelper.Clamp(projectile.ai[0], .015f, 1.25f);
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = (float)(110 + projectile.owner);
			}
			projectile.velocity *= .96f;
			projectile.velocity.Y += .016f;
		}

		public override void Kill(int timeLeft)
		{
			if(timeLeft > 0) //Dying through pierce/tile collision rather than naturally
			{
				if (!Main.dedServ)
					for (int i = 0; i < 6; i++)
						Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.05f, 0.2f), 
							Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f), 20));
			}

			for (int k = 0; k < 10; k++)
			{
				Dust d = Dust.NewDustPerfect(projectile.Center + (projectile.velocity / 10), DustID.DungeonSpirit, Main.rand.NextVector2Circular(2, 2) + (projectile.oldVelocity / 20), Scale : Main.rand.NextFloat(0.8f, 1.8f));
				d.noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(Color.White * .65f) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, Main.projectileTexture[projectile.type].Bounds, projectile.GetAlpha(Color.White * .9f), projectile.rotation,
				Main.projectileTexture[projectile.type].Size() / 2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
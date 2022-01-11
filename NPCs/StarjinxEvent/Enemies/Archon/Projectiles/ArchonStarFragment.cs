using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles
{
	public class ArchonStarFragment : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Fragment");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			projectile.hostile = true;
			projectile.timeLeft = 60;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.velocity *= 0.98f;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(projectile.Center, Color.LightCyan.ToVector3() / 3);

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f) * projectile.scale, 25));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch, 0.25f);
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;

		public void DoTrailCreation(TrailManager tManager)
		{
			float scalemod = projectile.scale;
			tManager.CreateTrail(projectile, new GradientTrail(new Color(145, 255, 253), new Color(61, 178, 224)), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 70f * scalemod, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(145, 255, 253) * .5f, new Color(61, 178, 224) * .5f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 140f * scalemod, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 10f * scalemod, new DefaultShader());
		}
	}
}
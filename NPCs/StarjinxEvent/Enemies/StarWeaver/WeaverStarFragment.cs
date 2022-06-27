using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class WeaverStarFragment : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Fragment");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10, 10);
			Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			Projectile.hostile = true;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0) //adjust scale based on ai, didnt work in setdefaults
			{
				Projectile.scale *= Projectile.ai[0];
				Projectile.localAI[0]++;
			}

			Projectile.tileCollide = Projectile.timeLeft < 50;
			Projectile.velocity *= 0.98f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() / 3);

			if(Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.White, Color.Goldenrod, Main.rand.NextFloat(0.1f, 0.2f) * Projectile.scale, 25));
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(spriteBatch, 0.25f);
			Projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

		public void DoTrailCreation(TrailManager tManager)
		{
			float scalemod = Projectile.scale * Projectile.ai[0];
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 215, 105), new Color(105, 213, 255)), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 70f * scalemod, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 215, 105) * .5f, new Color(105, 213, 255) * .5f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 140f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 10f * scalemod, new DefaultShader());
		}
	}
}
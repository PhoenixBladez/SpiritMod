using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Tides
{
	public class RyBolt : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'ylheian");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;

			projectile.damage = 3;
			projectile.light = .25f;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(181, 120, 255)), new RoundCap(), new DefaultTrailPosition(), 28f, 430f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.08f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(99, 64, 255, 100)), new RoundCap(), new DefaultTrailPosition(), 20f, 250f, new DefaultShader());
		}

		bool summoned = false;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (!summoned) {
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(projectile.Center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 7f;
				}
				summoned = true;
			}
			projectile.ai[0] += 1f;
			if (projectile.ai[0] == 16f) {
				projectile.ai[0] = 0f;
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(projectile.Center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 7f;
				}
			}
			if (Main.rand.Next(3) == 1) {
				Dust dust = Dust.NewDustPerfect(projectile.Center, 173);
				dust.velocity = Vector2.Zero;
				dust.noGravity = true;
			}
		}
	}
}

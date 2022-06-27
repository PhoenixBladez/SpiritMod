using Microsoft.Xna.Framework;
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
			Projectile.width = 8;
			Projectile.height = 8;

			Projectile.damage = 3;
			Projectile.light = .25f;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new StandardColorTrail(new Color(181, 120, 255)), new RoundCap(), new DefaultTrailPosition(), 28f, 430f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_3").Value, 0.08f, 1f, 1f));
			tM.CreateTrail(Projectile, new StandardColorTrail(new Color(99, 64, 255, 100)), new RoundCap(), new DefaultTrailPosition(), 20f, 250f, new DefaultShader());
		}

		bool summoned = false;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (!summoned) {
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 7f;
				}
				summoned = true;
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] == 16f) {
				Projectile.ai[0] = 0f;
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 7f;
				}
			}
			if (Main.rand.Next(3) == 1) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 173);
				dust.velocity = Vector2.Zero;
				dust.noGravity = true;
			}
		}
	}
}


using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class DuskLanceProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Lance");
		}

		int timer = 10;
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);

			AIType = ProjectileID.Trident;
		}

		public override void AI()
		{
			timer--;

			if (timer == 0 & Main.rand.Next(4) == 1) {
				SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<DuskApparition>(), Projectile.damage / 3 * 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 20;
			}
			Projectile.alpha -= 127;
			Player player = Main.player[Projectile.owner];
			Vector2 vector5 = player.RotatedRelativePoint(player.MountedCenter, true);
			float num15 = (float)player.itemAnimation / (float)player.itemAnimationMax;
			float num16 = 1f - num15;
			float num17 = Projectile.velocity.ToRotation();
			float num18 = Projectile.velocity.Length();
			float num19 = 22f;
			Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy((double)(3.14159274f + num16 * 6.28318548f), default) * new Vector2(num18, Projectile.ai[0]);
			Vector2 destination3 = vector5 + spinningpoint.RotatedBy((double)num17, default) + new Vector2(num18 + num19 + 40f, 0f).RotatedBy((double)num17, default);

			Vector2 value2 = player.DirectionTo(destination3);
			Vector2 vector9 = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			float num25 = 2f;
			for (int num26 = 0; (float)num26 < num25; num26++) {
				Dust dust4 = Dust.NewDustDirect(Projectile.Center, 14, 14, DustID.ShadowbeamStaff, 0f, 0f, 110, default, 1f);
				dust4.velocity = player.DirectionTo(dust4.position) * 2f;
				dust4.position = Projectile.Center + vector9.RotatedBy((double)(num16 * 6.28318548f * 2f + (float)num26 / num25 * 6.28318548f), default) * 10f;
				dust4.scale = 1f + 0.6f * Main.rand.NextFloat();
				Dust dust6 = dust4;
				dust6.velocity += vector9 * 3f;
				dust4.noGravity = true;
			}
			for (int j = 0; j < 1; j++) {
				if (Main.rand.Next(3) == 0) {
					Dust dust5 = Dust.NewDustDirect(Projectile.Center, 20, 20, DustID.ShadowbeamStaff, 0f, 0f, 110, default, 1f);
					dust5.velocity = player.DirectionTo(dust5.position) * 2f;
					dust5.position = Projectile.Center + value2 * -110f;
					dust5.scale = 0.45f + 0.4f * Main.rand.NextFloat();
					dust5.fadeIn = 0.7f + 0.4f * Main.rand.NextFloat();
					dust5.noGravity = true;
					dust5.noLight = true;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.ShadowFlame, 220, false);
		}

	}
}

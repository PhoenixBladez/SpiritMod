using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class GraspProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grasp");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Valor);
			aiType = ProjectileID.Valor;
			projectile.width = 16;
			projectile.height = 18;
			projectile.penetrate = 6;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<BCorrupt>(), 180);
			if (crit) {
				target.AddBuff(BuffID.ShadowFlame, 180);
			}
		}
		public override void AI()
		{
			{
				Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 4;
				projectile.velocity.X *= 1.005f;
				Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f)];
				newDust.position = position;
				newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
				newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default);
				newDust.fadeIn = 0.5f;
				newDust.noGravity = true;
				newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1)];
				newDust.position = position;
				newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
				newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default);
				newDust.fadeIn = 0.5F;
				newDust.noGravity = true;
			}
		}

	}
}
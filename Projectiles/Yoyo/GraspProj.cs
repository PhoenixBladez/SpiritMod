using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Valor);
			AIType = ProjectileID.Valor;
			Projectile.width = 16;
			Projectile.height = 18;
			Projectile.penetrate = 6;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<BloodCorrupt>(), 180);

			if (crit)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override void AI()
		{
			Vector2 position = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 4;
			Projectile.velocity.X *= 1.005f;
			Dust newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
		}
	}
}
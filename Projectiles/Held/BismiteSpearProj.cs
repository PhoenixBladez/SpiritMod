using SpiritMod.Buffs.DoT;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class BismiteSpearProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bismite Pike");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);
			aiType = ProjectileID.Trident;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Plantera_Green, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 0.9f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 180);
		}
	}
}

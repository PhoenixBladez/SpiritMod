using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.Projectiles.Returning
{
	public class BismiteCutter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Cutter");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.magic = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Plantera_Green, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Plantera_Green, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = 0.9f;
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

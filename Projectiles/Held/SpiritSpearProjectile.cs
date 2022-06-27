using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class SpiritSpearProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spear");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);

			AIType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<SoulBurn>(), 280);
		}

		int timer = 0;
		public override void AI()
		{
			timer--;

			if (timer == 0) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + 10, Projectile.velocity.Y, ModContent.ProjectileType<SoulSpirit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 60;
			}
		}

	}
}

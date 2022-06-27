
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class FlameTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flame Trail");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 1;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}

		public override bool PreAI()
		{
			int newDust = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height, DustID.Torch, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 100, default, 1f);
			Dust dust = Main.dust[newDust];
			dust.position.X = dust.position.X - 2f;
			dust.position.Y = dust.position.Y + 2f;
			dust.scale += (float)Main.rand.Next(50) * 0.01f;
			dust.noGravity = true;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60);
		}
	}
}

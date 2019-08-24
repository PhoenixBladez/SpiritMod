using System;

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
			projectile.width = 8;
			projectile.height = 8;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.aiStyle = 1;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.friendly = true;
		}

		public override bool PreAI()
		{
			int newDust = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height, DustID.Fire, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 100, default(Color), 1f);
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

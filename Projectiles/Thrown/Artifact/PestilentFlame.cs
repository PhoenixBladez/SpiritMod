using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Artifact
{
	public class PestilentFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necrotic Ember");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.thrown = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.timeLeft = 60;
			projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			int newDust = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height, 75, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 100, default(Color), 1f);
			Dust dust = Main.dust[newDust];
			dust.position.X = dust.position.X - 2f;
			dust.position.Y = dust.position.Y + 2f;
			dust.scale += (float)Main.rand.Next(50) * 0.01f;
			dust.noGravity = true;
			return true;
		}

		public override void AI()
		{
			int newDust1 = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height, mod.DustType("Pestilence"), Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 100, default(Color), 1f);
			Dust dust = Main.dust[newDust1];
			dust.position.X = dust.position.X - 2f;
			dust.position.Y = dust.position.Y + 2f;
			dust.scale += (float)Main.rand.Next(50) * 0.01f;
			dust.noGravity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("Necrosis"), 60);
		}

	}
}

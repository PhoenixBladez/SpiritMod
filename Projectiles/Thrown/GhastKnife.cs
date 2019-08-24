using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	class GhastKnife : ModProjectile
	{
		private int lastFrame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Knife");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.height = 22;
			projectile.thrown = true;
			projectile.width = 10;
			projectile.tileCollide = false;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int timer = 0;

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			for (int index1 = 0; index1 < 5; ++index1)
			{
				float num10 = projectile.velocity.X * 0.2f * (float)index1;
				float num20 = (float)-((double)projectile.velocity.Y * 0.200000002980232) * (float)index1;
				int index20 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 175, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index20].noGravity = true;
				Main.dust[index20].velocity *= 0.0f;
				Main.dust[index20].scale *= 1.1f;
				Main.dust[index20].position.X -= num10;
				Main.dust[index20].position.Y -= num20;
			}

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(mod.BuffType("SpectreFury"), 300);

			if (Main.rand.Next(7) == 0)
				target.StrikeNPC(projectile.damage / 3 * 2, 0f, 0, crit);
		}

	}
}

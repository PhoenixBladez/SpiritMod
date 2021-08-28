using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PoisonCloud : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Cloud");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = 6;
			projectile.tileCollide = true;
			projectile.timeLeft = 180;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0f) {
				projectile.localAI[0] = 1f;
				projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}

			projectile.velocity *= 0.95f;
			if (Main.rand.NextBool(3)) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, 0f, 0f);
				Main.dust[dust].noGravity = true;
				dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.Pestilence>(), 0f, 0f);
				Main.dust[dust].noGravity = true;
			}
			projectile.frameCounter++;
			if ((float)projectile.frameCounter >= 12f) {
				if (++projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
				projectile.frameCounter = 0;
			}
			if (projectile.penetrate <= 1 || projectile.timeLeft < 30) {
				if ((projectile.alpha += 10) >= 255)
					projectile.Kill();
			}
			else
				projectile.alpha = Math.Max(projectile.alpha - 7, 0);
		}
		public override bool CanDamage() => projectile.penetrate > 1;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Poisoned, 300);
		}
	}
}

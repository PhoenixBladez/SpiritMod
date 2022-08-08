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
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = 6;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0f) {
				Projectile.localAI[0] = 1f;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}

			Projectile.velocity *= 0.95f;
			if (Main.rand.NextBool(3)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f);
				Main.dust[dust].noGravity = true;
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.Pestilence>(), 0f, 0f);
				Main.dust[dust].noGravity = true;
			}
			Projectile.frameCounter++;
			if ((float)Projectile.frameCounter >= 12f) {
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
				Projectile.frameCounter = 0;
			}
			if (Projectile.penetrate <= 1 || Projectile.timeLeft < 30) {
				if ((Projectile.alpha += 10) >= 255)
					Projectile.Kill();
			}
			else
				Projectile.alpha = Math.Max(Projectile.alpha - 7, 0);
		}
		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.penetrate > 1;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.Poisoned, 300);
		}
	}
}

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	class SpectreBullet : ModProjectile
	{
		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public const float MAX_ANGLE_CHANGE = (float)Math.PI / 12;
		public const float ACCELERATION = 0.5f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (Projectile.ai[1] == 0) {
				Projectile.ai[0] = -1;
				Projectile.ai[1] = Projectile.velocity.Length();
			}

			NPC target = null;
			if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxNPCs) {
				target = ProjectileExtras.FindCheapestNPC(Projectile.Center, Projectile.velocity, ACCELERATION, MAX_ANGLE_CHANGE);
			}
			else {
				target = Main.npc[(int)Projectile.ai[0]];
				if (!target.active || !target.CanBeChasedBy()) {
					target = ProjectileExtras.FindCheapestNPC(Projectile.Center, Projectile.velocity, ACCELERATION, MAX_ANGLE_CHANGE);
				}
			}

			if (target == null) {
				Projectile.ai[0] = -1f;
			}
			else {
				Projectile.ai[0] = (float)target.whoAmI;
				ProjectileExtras.HomingAI(this, target, Projectile.ai[1], ACCELERATION);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.chaseable || target.lifeMax <= 5 || target.dontTakeDamage || target.friendly || target.immortal)
				return;

			if (Main.rand.Next(100) <= 35) {
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, Projectile.owner, Projectile.owner, 1);
			}
		}

		public override void PostDraw(Color lightColor)
		{
			int max = (int)(Projectile.ai[1] * .4f);
			Vector2 offset = Projectile.velocity * ((Projectile.extraUpdates + 1f) / max);
			float vX = Projectile.velocity.X * (Projectile.extraUpdates + 1);
			float vY = Projectile.velocity.Y * (Projectile.extraUpdates + 1);
			for (int i = 0; i < max; i++) {
				Vector2 position = Projectile.Center - offset * i;
				int dust = Dust.NewDust(position, 0, 0, DustID.Flare_Blue, vX * .25f - vY * .08f, vY * .25f + vX * .08f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 0.9f;
				dust = Dust.NewDust(position, 0, 0, DustID.Flare_Blue, vX * .25f + vY * .08f, vY * .25f - vX * .08f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}

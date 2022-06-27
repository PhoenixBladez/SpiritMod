
using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Glyph;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SlicingGust : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Slicing Gust");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 220;
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0f)
			{
				ProjectileExtras.LookAlongVelocity(this);
				Projectile.localAI[0] = 1f;
			}

			if (Main.rand.NextDouble() < 0.5)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(4, 4), Projectile.width + 8, Projectile.height + 8, ModContent.DustType<Wind>());
				dust.velocity = Projectile.velocity * 0.2f;
				dust.customData = new WindAnchor(Projectile.Center, Projectile.velocity, dust.position);
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitDirection = 0;
			knockback = 0.12f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.velocity.Y -= Projectile.knockBack * target.knockBackResist;
			target.AddBuff(ModContent.BuffType<WindBurst>(), 300);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Wind>());
				dust.customData = new WindAnchor(Projectile.Center, Projectile.velocity, dust.position);
			}
		}
	}
}

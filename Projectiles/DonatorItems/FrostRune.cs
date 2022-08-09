using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class FrostRune : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Rune");
		}

		public override void SetDefaults()
		{
			Projectile.width = 66;
			Projectile.height = 66;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			AIType = ProjectileID.Mushroom;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;

			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Frostburn, 200, true);
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
		}

	}
}
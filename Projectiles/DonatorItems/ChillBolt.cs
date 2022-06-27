using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class ChillBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chilly Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 9;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			for (int I = 0; I < 8; I++)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 3; ++i) {
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FrostRune>(), Projectile.damage, 0, Projectile.owner);
			}
		}
		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 1)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.Frostburn, 200, true);

			for (int i = 0; i < 3; ++i) {
				int randFire = Main.rand.Next(1);
				int newProj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, Main.rand.Next(0) / 100, Main.rand.Next(0, 0), ModContent.ProjectileType<FrostRune>(), Projectile.damage/3 * 2, 0, Projectile.owner);
			}
		}
	}
}
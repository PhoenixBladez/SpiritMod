using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Blood3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Cluster");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 255;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else {
				AIType = ProjectileID.Shuriken;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
			}
			return false;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 6; k++) {
				int index2 = Dust.NewDust(Projectile.position, 4, Projectile.height, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .8f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			if (Main.rand.Next(18) <= 9 && player.statLife != player.statLifeMax2) {
				int lifeToHeal = 0;

				if (player.statLife + 1 <= player.statLifeMax2)
					lifeToHeal = 2;
				else
					lifeToHeal = player.statLifeMax2 - player.statLife;

				player.statLife += lifeToHeal;
				player.HealEffect(lifeToHeal);
			}
		}
	}
}
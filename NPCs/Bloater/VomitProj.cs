using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Bloater
{
	public class VomitProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Vomit");
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			AIType = ProjectileID.Flames;
			Projectile.alpha = 255;
			Projectile.timeLeft = 22;
			Projectile.penetrate = 4;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.5f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}

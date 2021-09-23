using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteWall : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Wall");

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 24;
			projectile.height = 88;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.tileCollide = false;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Granite, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return false;
		}
	}
}

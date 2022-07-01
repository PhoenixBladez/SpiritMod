using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mangrove_Defender
{
	public class Earth_Slam_Invisible : ModProjectile
	{
		public int projectileTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earth Slam");
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.hide = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.timeLeft = 121;
		}

		public override void AI()
		{
			projectileTimer++;
			if (projectileTimer % 30 == 0)
			{
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0f, 20f, ModContent.ProjectileType<Earth_Slam_Falling>(), 0, 0, Projectile.owner);
				Main.projectile[p].tileCollide = true;
				Main.projectile[p].hostile = false;
			}
		}
	}
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mangrove_Defender
{
	public class Earth_Slam_Falling : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Earth Slam");

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.hide = true;
			projectile.scale = 1f;
		}

		public override void Kill(int timeLeft) => Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 40, 0f, 0f, ModContent.ProjectileType<Earth_Slam_Projectile>(), 23, 10f, projectile.owner);
	}
}
using Microsoft.Xna.Framework;
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
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.hide = true;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.timeLeft = 121;
		}
		public override void AI()
		{
			projectileTimer++;
			if (projectileTimer % 30 == 0)
			{
				int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 20f, mod.ProjectileType("Earth_Slam_Falling"), 0, 0, projectile.owner);
				Main.projectile[p].tileCollide = true;
				Main.projectile[p].hostile = false;
			}
		}
	}
}
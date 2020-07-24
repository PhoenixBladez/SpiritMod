using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class SkellyP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Yelets);
			projectile.damage = 56;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Yelets;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localAI[1] += 1f;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if (projectile.localAI[1] <= 1.0) {
				int num3 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<PrimeOther>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num3].localAI[0] = projectile.whoAmI;
				return;
			}
			int num4 = (int)projectile.localAI[1];
			if (num4 <= 30) {
				if (num4 != 10) {
					if (num4 == 30) {
						num2--;
					}
				}
				else {
					num2--;
				}
			}
			else if (num4 != 50) {
				if (num4 == 70) {
					num2--;
				}
			}
			else {
				num2--;
			}
			if (new int[]
			{
				20
			}.Contains((int)projectile.localAI[1])) {
				int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<PrimeSaw>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = projectile.whoAmI;
			}
			if (new int[]
			{
				30
			}.Contains((int)projectile.localAI[1])) {
				int num6 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<PrimeVice>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num6].localAI[0] = projectile.whoAmI;
			}
			if (new int[]
			{
				40
			}.Contains((int)projectile.localAI[1])) {
				int num7 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<PrimeLaser>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num7].localAI[0] = projectile.whoAmI;
			}
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
		}
	}
}

using System;
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
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(549);
			base.projectile.damage = 56;
			base.projectile.extraUpdates = 1;
			this.aiType = 549;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			base.projectile.localAI[1] += 1f;
		}

		public override void AI()
		{
			base.projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if ((double)base.projectile.localAI[1] <= 1.0)
			{
				int num3 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)num, (float)num2, base.mod.ProjectileType("PrimeOther"), base.projectile.damage, base.projectile.knockBack, base.projectile.owner, 0f, 0f);
				Main.projectile[num3].localAI[0] = (float)base.projectile.whoAmI;
				return;
			}
			int num4 = (int)base.projectile.localAI[1];
			if (num4 <= 30)
			{
				if (num4 != 10)
				{
					if (num4 == 30)
					{
						num2--;
					}
				}
				else
				{
					num2--;
				}
			}
			else if (num4 != 50)
			{
				if (num4 == 70)
				{
					num2--;
				}
			}
			else
			{
				num2--;
			}
			if (new int[]
			{
				20
			}.Contains((int)base.projectile.localAI[1]))
			{
				int num5 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)num, (float)num2, base.mod.ProjectileType("PrimeSaw"), base.projectile.damage, base.projectile.knockBack, base.projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = (float)base.projectile.whoAmI;
			}
			if (new int[]
			{
				30
			}.Contains((int)base.projectile.localAI[1]))
			{
				int num6 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)num, (float)num2, base.mod.ProjectileType("PrimeVice"), base.projectile.damage, base.projectile.knockBack, base.projectile.owner, 0f, 0f);
				Main.projectile[num6].localAI[0] = (float)base.projectile.whoAmI;
			}
			if (new int[]
			{
				40
			}.Contains((int)base.projectile.localAI[1]))
			{
				int num7 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)num, (float)num2, base.mod.ProjectileType("PrimeLaser"), base.projectile.damage, base.projectile.knockBack, base.projectile.owner, 0f, 0f);
				Main.projectile[num7].localAI[0] = (float)base.projectile.whoAmI;
			}
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}
	}
}

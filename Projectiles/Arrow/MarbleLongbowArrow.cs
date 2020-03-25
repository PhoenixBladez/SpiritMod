using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class MarbleLongbowArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Longbow");
		}

		public override void SetDefaults()
		{
            projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            projectile.width = 6;
            projectile.height = 12;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		public override void AI()
		{
            int num = 5;
            for (int k = 0; k < 3; k++)
            {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 222, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            projectile.Kill();

        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            int d = 236;
            for (int k = 0; k < 6; k++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.27f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.37f);
            }
            int num2 = Main.rand.Next(2, 4);
            int num3 = Main.rand.Next(0, 360);
            int num24 = mod.ProjectileType("MarbleArrowStone");
            for (int j = 0; j < num2; j++)
            {
                float num4 = MathHelper.ToRadians((float)(270 / num2 * j + num3));
                Vector2 vector = new Vector2(base.projectile.velocity.X, base.projectile.velocity.Y).RotatedBy((double)num4, default(Vector2));
                vector.Normalize();
                vector.X *= 6.5f;
                vector.Y *= 6.5f;
                int p = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, num24, projectile.damage / 5 * 2, 0f, 0);
                Main.projectile[p].hostile = false;
                Main.projectile[p].friendly = true;
            }
        }
    }
}

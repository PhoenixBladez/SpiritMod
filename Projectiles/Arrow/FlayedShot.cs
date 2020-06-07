using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class FlayedShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flayed Bolt");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 14;
			projectile.height = 30;
		}
        public override void AI()
        {
            int num = 5;
            for (int k = 0; k < 5; k++)
            {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 5, 0.0f, 0.0f, 0, new Color(), 1.6f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = 1.5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		    target.AddBuff(ModContent.BuffType<BloodInfusion>(), 900, true);
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<NightmareDust>());
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
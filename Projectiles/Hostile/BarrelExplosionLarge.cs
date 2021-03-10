using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class BarrelExplosionLarge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
			Main.projFrames[base.projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 149;
			projectile.height = 170;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}
	
    	public override void AI()
        {
            if (projectile.timeLeft == 29)
            {
            Main.PlaySound(2, projectile.Center, 14);
            Main.PlaySound(3, projectile.Center, 4);
            for (int num625 = 0; num625 < 2; num625++)
            {
                float scaleFactor10 = 0.33f;
                if (num625 == 1)
                    scaleFactor10 = 0.66f;
                else if (num625 == 2)
                    scaleFactor10 = 1f;

                int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13C3C_cp_0 = Main.gore[num626];
                expr_13C3C_cp_0.velocity.X = expr_13C3C_cp_0.velocity.X + 1f;
                Gore expr_13C5C_cp_0 = Main.gore[num626];
                expr_13C5C_cp_0.velocity.Y = expr_13C5C_cp_0.velocity.Y - 1f;
            }
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 6, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.875f, 1.61f));
            }
            }
            projectile.frameCounter++;
			if (projectile.frameCounter >= 3) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 10)
					projectile.frame = 0;
			}
        }
        public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
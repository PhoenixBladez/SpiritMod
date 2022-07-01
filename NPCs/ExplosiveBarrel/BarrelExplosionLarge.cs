using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ExplosiveBarrel
{
	public class BarrelExplosionLarge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
			Main.projFrames[base.Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 149;
			Projectile.height = 170;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}
	
    	public override void AI()
        {
            if (Projectile.timeLeft == 29)
            {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            for (int num625 = 0; num625 < 2; num625++)
            {
                float scaleFactor10 = 0.33f;
                if (num625 == 1)
                    scaleFactor10 = 0.66f;
                else if (num625 == 2)
                    scaleFactor10 = 1f;

                int num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13C3C_cp_0 = Main.gore[num626];
                expr_13C3C_cp_0.velocity.X = expr_13C3C_cp_0.velocity.X + 1f;
                Gore expr_13C5C_cp_0 = Main.gore[num626];
                expr_13C5C_cp_0.velocity.Y = expr_13C5C_cp_0.velocity.Y - 1f;
            }
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 6, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.875f, 1.61f));
            }
            }
            Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 10)
					Projectile.frame = 0;
			}
        }
        public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
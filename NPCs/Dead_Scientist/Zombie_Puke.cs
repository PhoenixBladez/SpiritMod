using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dead_Scientist
{
	public class Zombie_Puke : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie's Puke");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 0;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 30;
			projectile.scale = 1f;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			
			float scale = Main.rand.Next(10)== 0 ? 2f : 0.8f;
            for (int index1 = 0; index1 < 2; ++index1)
            {
                int index2 = Dust.NewDust(projectile.Center, 0, 0, DustID.Blood, 0.0f, 0.0f, 0, new Color(), scale);
                Main.dust[index2].velocity *= 1.2f;
                Main.dust[index2].fadeIn = 0.72f;
                --Main.dust[index2].velocity.Y;
                Main.dust[index2].velocity += new Vector2(projectile.velocity.X*2, projectile.velocity.Y);
                Main.dust[index2].noGravity = true;
            }
		}
		
		public override void Kill(int timeLeft)
		{
		}
	}
}
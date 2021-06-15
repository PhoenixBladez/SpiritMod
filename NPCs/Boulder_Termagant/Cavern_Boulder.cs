using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Cavern_Boulder : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Boulder");
		}
		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.penetrate = 100;
		}

		public override void AI()
		{
			Player player = Main.LocalPlayer;
			for (int index = 0; index < 6; ++index)
			{
				if (Main.rand.Next(10) != 0)
				{
					Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 1, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + projectile.velocity / 2f;
					dust.scale = (float)(0.800000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
					dust.position = projectile.Center;
					dust.position += new Vector2((float)(projectile.width/2), 0.0f).RotatedBy(6.28318548202515 * (double)Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = true;
					dust.noGravity = true;
				}
			}
			if (projectile.position.Y > player.position.Y)
				projectile.tileCollide = true;
		}
		
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 156, 0f, -2f, 175, default(Color), .6f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center) {
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
		}
	}
}
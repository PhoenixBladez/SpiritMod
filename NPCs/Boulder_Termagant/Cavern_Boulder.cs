using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = 100;
		}

		public override void AI()
		{
			Player player = Main.LocalPlayer;
			for (int index = 0; index < 6; ++index)
			{
				if (Main.rand.Next(10) != 0)
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + Projectile.velocity / 2f;
					dust.scale = (float)(0.800000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
					dust.position = Projectile.Center;
					dust.position += new Vector2((float)(Projectile.width/2), 0.0f).RotatedBy(6.28318548202515 * (double)Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = true;
					dust.noGravity = true;
				}
			}
			if (Projectile.position.Y > player.position.Y)
				Projectile.tileCollide = true;
		}
		
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UltraBrightTorch, 0f, -2f, 175, default, .6f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != Projectile.Center) {
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0f);
		}
	}
}
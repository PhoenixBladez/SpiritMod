using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Marble_Boulder : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Pillar");
		}
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 44;
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
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + Projectile.velocity / 2f;
					dust.scale = (float)(0.800000011920929 + Main.rand.NextFloat() * 0.400000005960464);
					dust.position = Projectile.Center;
					dust.position += new Vector2((Projectile.width/2), 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = true;
					dust.noGravity = true;
				}
			}
			if (Projectile.position.Y > player.position.Y)
				Projectile.tileCollide = true;
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
	}
}
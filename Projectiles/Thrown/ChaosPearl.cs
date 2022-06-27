
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class ChaosPearl : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Windshear Pearl");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = 2;
			Projectile.damage = 0;
			//projectile.thrown = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
		}

		public override bool PreAI()
		{
			Projectile.rotation += 0.1f;

			return true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			SoundEngine.PlaySound((int)Projectile.position.X, (int)Projectile.position.Y, 27);
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 8);
			//Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			for (int num424 = 0; num424 < 10; num424++) {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Firework_Yellow, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.75f);
			}
			Main.player[Projectile.owner].Teleport(new Vector2(Projectile.position.X, Projectile.position.Y - 32), 2, 0);
			if (Main.player[Projectile.owner].FindBuffIndex(88) >= 0) {
				player.statLife -= (player.statLifeMax2 / 7);
				if (player.statLife <= 0) {
					player.statLife = 1;
					player.AddBuff(BuffID.Suffocation, 120);
					//    player.KillMe(9999, 1, true, "'s head appeared where their legs should be.");
				}
			}
			Main.player[Projectile.owner].AddBuff(88, 240);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}

	}
}

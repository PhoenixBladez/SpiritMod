using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Pet
{
	public class Lantern : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lantern");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			Projectile.width = 46;
			Projectile.height = 46;
			Projectile.scale = 0.9f;
		}

		public override bool PreAI()
		{
			int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Green, 0, -1f, 0, default, 1f);
			Main.dust[d].scale *= 0.5f;
			Main.dust[d].noGravity = true;
			Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.75f / 2, 1.5f / 2, 0.75f / 2);

			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			var modPlayer = player.GetModPlayer<GlobalClasses.Players.PetPlayer>();
			if (player.dead)
				modPlayer.lanternPet = false;

			if (modPlayer.lanternPet)
				Projectile.timeLeft = 2;

			if (player.controlDown && player.releaseDown)
			{
				if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
				{
					Vector2 vectorToMouse = Main.MouseWorld - Projectile.Center;
					Projectile.velocity += 5f * Vector2.Normalize(vectorToMouse);
				}
			}
		}
	}
}
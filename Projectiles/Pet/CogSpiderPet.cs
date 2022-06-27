using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Pet
{
	public class CogSpiderPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Spider");
			Main.projFrames[Projectile.type] = 3;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Penguin);
			AIType = ProjectileID.Penguin;
			Projectile.width = 54;
			Projectile.height = 30;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.penguin = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.starPet = false;

			if (modPlayer.starPet)
				Projectile.timeLeft = 2;
		}

	}
}
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
			Main.projFrames[projectile.type] = 3;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Penguin);
			aiType = ProjectileID.Penguin;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.penguin = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.dead)
				modPlayer.starPet = false;

			if (modPlayer.starPet)
				projectile.timeLeft = 2;
		}

	}
}
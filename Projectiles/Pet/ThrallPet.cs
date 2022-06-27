using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class ThrallPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Leonardo");
			Main.projFrames[Projectile.type] = 7;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabySnowman);
			AIType = ProjectileID.BabySnowman;
			Projectile.height = 40;
			Projectile.width = 20;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.snowman = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.thrallPet = false;

			if (modPlayer.thrallPet)
				Projectile.timeLeft = 2;

		}

	}
}
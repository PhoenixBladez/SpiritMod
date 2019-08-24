using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Pet
{
	public class ShadowPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Pup");
			Main.projFrames[projectile.type] = 12;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Truffle);
			aiType = ProjectileID.Truffle;
			projectile.width = 30;
			projectile.height = 36;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.truffle = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.dead)
				modPlayer.shadowPet = false;
			if (modPlayer.shadowPet)
				projectile.timeLeft = 2;
		}

	}
}
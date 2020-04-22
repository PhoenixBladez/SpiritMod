using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Pet
{
	public class BriarSlimePet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Briarthorn Slime");
			Main.projFrames[projectile.type] = 12;
			Main.projPet[projectile.type] = true;
		}

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Truffle);
            aiType = ProjectileID.Truffle;
            projectile.width = 30;
            projectile.height = 20;
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
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.briarSlimePet = false;
			if (modPlayer.briarSlimePet)
				projectile.timeLeft = 2;
		}

	}
}
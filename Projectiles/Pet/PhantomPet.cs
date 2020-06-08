using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Pet
{
    public class PhantomPet : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phantom");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
        }

        public override bool PreAI() {
            Player player = Main.player[projectile.owner];
            player.zephyrfish = false; // Relic from aiType
            return true;
        }

        public override void AI() {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(player.dead)
                modPlayer.phantomPet = false;

            if(modPlayer.phantomPet)
                projectile.timeLeft = 2;
        }

    }
}
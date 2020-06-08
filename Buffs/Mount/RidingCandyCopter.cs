using SpiritMod.Mounts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
    class RidingCandyCopter : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Candy Copter");
            Description.SetDefault("GET TO THE CHOPPER!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.mount.SetMount(ModContent.MountType<CandyCopter>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}

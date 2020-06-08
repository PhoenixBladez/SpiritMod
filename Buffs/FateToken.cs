using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class FateToken : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Fate's Blessing");
            Description.SetDefault("You are protected by the fates");
            Main.buffNoSave[Type] = true;
            canBeCleared = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            player.buffTime[buffIndex] = modPlayer.timeLeft;
        }
    }
}

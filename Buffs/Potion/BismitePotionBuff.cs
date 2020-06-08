using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
    public class BismitePotionBuff : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Toxin Strike");
            Description.SetDefault("Critical strikes may Poison hit foes\nIncreases critical strike chance by 4%");
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            modPlayer.poisonPotion = true;
            player.meleeCrit += 4;
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
        }
    }
}

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
    public class RogueCooldown : ModBuff
    {
        public override void SetDefaults() {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Dispelled Shadow");
            Description.SetDefault("'Wait for the darkness to steal you away'");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
    }
}
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class UnPowered : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Power Loss");
            Description.SetDefault("You cannot utilize the Darkfire Katana");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
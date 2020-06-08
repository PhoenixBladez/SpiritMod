using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    class TikiInfestation : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Tiki Infestation");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}

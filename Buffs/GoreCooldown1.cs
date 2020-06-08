using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class GoreCooldown1 : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Gore Cooldown");
            Description.SetDefault("The blood of gods must seep back...");
            Main.buffNoTimeDisplay[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }
    }
}
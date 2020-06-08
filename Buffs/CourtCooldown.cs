using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class CourtCooldown : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Sanguine Cooldown");
            Description.SetDefault("Your blood needs time to heal");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }
    }
}

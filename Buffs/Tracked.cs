using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Tracked : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Tracked");
            Main.buffNoTimeDisplay[Type] = false;
            Main.pvpBuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {

            {
                npc.GetGlobalNPC<GNPC>().tracked = true;

            }
        }
    }
}
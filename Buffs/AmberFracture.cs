using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class AmberFracture : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Amber Fracture");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<GNPC>().amberFracture = true;
        }
    }
}

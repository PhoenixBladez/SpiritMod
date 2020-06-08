using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class StackingFireBuff : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Combustion Blaze");
        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {
            GNPC info = npc.GetGlobalNPC<GNPC>();
            if(info.fireStacks < 3) {
                info.fireStacks++;
            }

            npc.buffTime[buffIndex] = time;

            return true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            GNPC info = npc.GetGlobalNPC<GNPC>();
            if(info.fireStacks <= 0) {
                info.fireStacks = 1;
            }

            if(Main.rand.NextBool(2)) {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
            }
        }
    }
}

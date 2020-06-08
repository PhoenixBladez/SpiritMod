using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class TidalEbb : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Tidal Ebb");
            Main.buffNoTimeDisplay[Type] = false;
            Main.pvpBuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.defense = (int)(npc.defense * 0.95f);
            npc.lifeRegen -= 1;

            Dust.NewDust(npc.position, npc.width, npc.height, 172);
        }
    }
}
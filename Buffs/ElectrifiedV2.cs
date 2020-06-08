using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class ElectrifiedV2 : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Electrified");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.lifeRegen -= 30;

            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
        }
    }
}

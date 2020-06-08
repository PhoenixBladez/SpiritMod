using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class ClatterPierce : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Clatter Break");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<GNPC>().clatterPierce = true;
            npc.defense -= 3;

            for(int k = 0; k < 2; k++) {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Honey);
                Main.dust[dust].scale *= .52f;
            }
        }
    }
}

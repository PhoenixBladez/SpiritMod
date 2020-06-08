using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class PollinationPoison : ModBuff
    {
        public override void SetDefaults() {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Pollination Poison");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.lifeRegen -= 7;

            if(Main.rand.NextBool(2)) {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 107);
                Main.dust[dust].scale = 0.5f;
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
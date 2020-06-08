using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Distorted : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Distorted");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            if(!npc.boss) {
                npc.velocity.Y = -3;

                if(Main.rand.NextBool(2)) {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 110);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }
    }
}

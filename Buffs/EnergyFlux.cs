using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class EnergyFlux : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Granite Energy Flux");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            if(!npc.boss && Main.rand.NextBool(4)) {
                npc.velocity.Y *= 0.2f;
            } else if(!npc.boss && Main.rand.NextBool(4)) {
                npc.velocity.Y *= 1.1f;
            }

            if(!npc.boss && Main.rand.NextBool(4)) {
                npc.velocity.X *= .3f;
            } else if(!npc.boss && Main.rand.NextBool(4)) {
                npc.velocity.X *= .5f;
            }

            if(Main.rand.NextBool(2)) {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].noGravity = true;
            }
        }

    }
}

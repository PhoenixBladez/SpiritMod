using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Frozen : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Frozen");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            if(!npc.boss) {
                npc.velocity.X *= 0f;
                npc.velocity.Y *= 0f;

                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueCrystalShard);
                Main.dust[dust].scale = 1.9f;
                Main.dust[dust].velocity *= 3f;
                Main.dust[dust].noGravity = true;
            }
        }

    }
}

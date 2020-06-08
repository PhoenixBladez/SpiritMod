using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Marked : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Marked");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.CopperCoin);
            Main.dust[dust].scale = 2.9f;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].noGravity = true;
        }
    }
}

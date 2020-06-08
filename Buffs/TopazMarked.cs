using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class TopazMarked : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Illuminated");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
            Main.dust[dust].scale = Main.rand.NextFloat(.4f, .8f);
            Main.dust[dust].velocity.Y *= -1f;
            Main.dust[dust].velocity.X *= 0f;
            Main.dust[dust].noGravity = true;
        }
    }
}

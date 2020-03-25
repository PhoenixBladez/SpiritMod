using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Stopped : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Stopped");
            Description.SetDefault("You are locked in place");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GNPC>().Stopped = true;

            if (!npc.boss)
            {
                npc.velocity *= 0;
                npc.frame.Y = 0;
            }

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 206);
                Main.dust[dust].scale = 1f;
            }
        }
    }
}
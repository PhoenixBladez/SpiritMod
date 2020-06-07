using SpiritMod.Dusts;
using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class BloodInfusion : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Blood Infusion");
            Description.SetDefault("'Extremely contagious'");
            Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().bloodInfusion = true;
			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<NightmareDust>());
				Main.dust[dust].scale = 1.2f;
				Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }
		}
	}
}
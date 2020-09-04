using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.SummonTag
{
	public class SacrificialDaggerBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sacrificial Strike");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().sacrificialDaggerBuff = true;
		}

	}
}
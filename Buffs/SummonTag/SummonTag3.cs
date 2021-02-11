using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.SummonTag
{
	public class SummonTag3 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Summon Tag");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex) => npc.GetGlobalNPC<GNPC>().summonTag = 3;
	}
}
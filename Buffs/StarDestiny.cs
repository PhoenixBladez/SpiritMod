using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class StarDestiny : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Destiny");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().starDestiny = true;
		}
	}
}

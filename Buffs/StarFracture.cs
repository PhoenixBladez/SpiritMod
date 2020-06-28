using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class StarFracture : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Fracture");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().sFracture = true;
			npc.defense -= 4;
		}
	}
}
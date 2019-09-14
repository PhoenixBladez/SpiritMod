using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Death : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Death");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				npc.GetGlobalNPC<GNPC>(mod).Death = true;
			}
		}
	}
}

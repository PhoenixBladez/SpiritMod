using Terraria;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class DoomDestiny1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Quicksilver Cut");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).DoomDestiny1 = true;
		}
	}
}

using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class DoomDestiny : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Cut");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetSpiritPlayer().DoomDestiny = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).DoomDestiny = true;
		}
	}
}

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class HolyLight : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Holy Light");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = (int)(npc.defense * 0.87f);

			Dust.NewDust(npc.position, npc.width, npc.height, DustID.Enchanted_Pink);
		}
	}
}

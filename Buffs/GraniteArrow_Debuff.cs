using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class GraniteArrow_Debuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Arrow Debuff");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.lifeRegen > 0) {
				npc.lifeRegen = 0;
			}

			npc.lifeRegen -= 12;
		}
	}
}

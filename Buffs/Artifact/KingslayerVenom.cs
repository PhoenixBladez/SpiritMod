using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class KingslayerVenom : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Kingslayer Venom");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = (npc.defDefense / 100) * 82;
			npc.lifeRegen -= 8;

			int p = Dust.NewDust(npc.position, npc.width, npc.height, 110);
			Main.dust[p].velocity *= 0f;
		}
	}
}

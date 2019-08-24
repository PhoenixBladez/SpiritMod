using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class BCorrupt : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Blood Corruption");
			Description.SetDefault("Your blood burns...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.friendly)
			{
				if (npc.lifeRegen > 0)
					npc.lifeRegen = 0;
				npc.lifeRegen -= 4;

				Dust.NewDust(npc.position, npc.width, npc.height, 5);
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;
			player.lifeRegen -= 4;

			Dust.NewDust(player.position, player.width, player.height, 5);
		}
	}
}

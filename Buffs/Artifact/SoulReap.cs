using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class SoulReap : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Reap");
			Description.SetDefault("The souls of your enemies increase Life Regeneration");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 4;
		}
	}
}

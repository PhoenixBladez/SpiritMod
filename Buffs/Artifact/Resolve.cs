using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class Resolve : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Unyielding Resolve");
			Description.SetDefault("'You must go on'");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 3;
			player.endurance += 0.05f;
		}
	}
}

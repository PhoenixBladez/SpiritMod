using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class VirulenceCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Virulent Suppression");
			Description.SetDefault("'The Putrid Humors must reset their energy'");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class InfernalRage : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Infernal Rage");
			Description.SetDefault("Greatly boosted damage at the cost of your soul...");
			Main.buffNoTimeDisplay[Type] = true;
		}
	}
}

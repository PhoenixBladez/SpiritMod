using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class DesertWinds : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Desert Winds");
			Description.SetDefault("The ancient winds flow through you...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}
	}
}
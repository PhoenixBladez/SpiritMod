using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class OverchargedOrion : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Overcharged");
			Description.SetDefault("Orion's quickdraw is now overcharged");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}

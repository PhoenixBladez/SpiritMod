using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class ShieldZoneTimer : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fortification Zone");
			Description.SetDefault("The Fortification Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

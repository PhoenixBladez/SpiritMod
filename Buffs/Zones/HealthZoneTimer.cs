using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class HealthZoneTimer : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Healing Zone");
			Description.SetDefault("The Healing Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

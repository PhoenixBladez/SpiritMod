using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class CryoZoneTimer : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slow Zone");
			Description.SetDefault("The Slow Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

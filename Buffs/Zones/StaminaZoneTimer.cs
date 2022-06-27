using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class StaminaZoneTimer : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stamina Zone");
			Description.SetDefault("The Stamina Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

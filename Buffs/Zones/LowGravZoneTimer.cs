using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class LowGravZoneTimer : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Low Gravity Zone");
			Description.SetDefault("The Low Gravity Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

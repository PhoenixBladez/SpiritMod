using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class RepulsionZoneTimer : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Repulsion Zone");
			Description.SetDefault("The Repulsion Zone is up!");
			Main.pvpBuff[Type] = true;
		}
	}
}

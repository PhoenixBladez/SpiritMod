using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class CrimsonSkullBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Skull");
			Description.SetDefault("Flask of gore now deals double damage");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}

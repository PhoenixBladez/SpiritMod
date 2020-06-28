using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	public class GraniteBonus : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shockwave");
			Description.SetDefault("'Geronimo!'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}
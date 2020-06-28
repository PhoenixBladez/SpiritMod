using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	public class MarbleDivineWinds : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Divine Winds");
			Description.SetDefault("'To the skies!'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}
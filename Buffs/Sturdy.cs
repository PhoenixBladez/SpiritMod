using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Sturdy : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sturdy");
			Description.SetDefault("'Your shell has been cracked'");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}
	}
}

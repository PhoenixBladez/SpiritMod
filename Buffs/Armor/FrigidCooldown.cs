using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	public class FrigidCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Below Zero");
			Description.SetDefault("'It's too cold to keep this wall going...'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}
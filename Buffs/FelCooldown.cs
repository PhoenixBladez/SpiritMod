using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class FelCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Fel Cooldown");
			Description.SetDefault("Cursed...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
	}
}

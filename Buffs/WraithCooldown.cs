using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class WraithCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wraith Cooldown");
			Description.SetDefault("From the shadows...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}

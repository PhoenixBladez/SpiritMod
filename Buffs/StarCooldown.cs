using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class StarCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Cooldown");
			Description.SetDefault("The cosmic energies must stabilize...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}
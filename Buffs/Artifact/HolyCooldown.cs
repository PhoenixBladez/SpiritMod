using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class HolyCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Holy Cooldown");
			Description.SetDefault("The Divine Energies must rest...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			longerExpertDebuff = false;
		}
	}
}

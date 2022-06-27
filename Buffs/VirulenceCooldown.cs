using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class VirulenceCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Suppression");
			Description.SetDefault("'The Putrid Humors must reset their energy'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class DeathRoseCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Death Rose Cooldown");
			Description.SetDefault("The bramble needs time to regrow...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
	}
}

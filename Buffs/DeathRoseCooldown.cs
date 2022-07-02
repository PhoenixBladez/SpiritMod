using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class DeathRoseCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Rose Cooldown");
			Description.SetDefault("The bramble needs time to regrow...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
	}
}

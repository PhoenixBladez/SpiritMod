using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AstralMapCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Cooldown");
			Description.SetDefault("Lightspeed travel is unstable!");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
	}
}

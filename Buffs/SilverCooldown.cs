using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SilverCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Quicksilver Cooldown");
			Description.SetDefault("You must wait to harness the souls...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}
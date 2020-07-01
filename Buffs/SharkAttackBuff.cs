using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SharkAttackBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mech Shark Cooldown");
			Description.SetDefault("You've run out of sharks!");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
	}
}
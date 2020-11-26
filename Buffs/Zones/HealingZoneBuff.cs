using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class HealingZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Healing Zone");
			Description.SetDefault("You feel invigorated!");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.lifeRegen += 3;
		}
	}
}

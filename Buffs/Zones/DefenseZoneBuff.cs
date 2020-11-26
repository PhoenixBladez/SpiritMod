using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class DefenseZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Fortification Zone");
			Description.SetDefault("You feel strong!");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.statDefense += 6;
            player.endurance += .1f;
		}
	}
}

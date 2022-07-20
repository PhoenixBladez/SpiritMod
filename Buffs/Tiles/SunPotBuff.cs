using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Tiles
{
	public class SunPotBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun in a Pot");
			Description.SetDefault("Increased life regeneration");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) => player.lifeRegen += 2;
	}
}

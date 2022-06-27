using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Candy
{
	public class ManaBuffC : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Candy");
			Description.SetDefault("+40 Mana");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statManaMax2 += 40;
		}
	}
}

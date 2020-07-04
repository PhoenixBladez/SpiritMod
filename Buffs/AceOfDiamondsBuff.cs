using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AceOfDiamondsBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ace of Diamonds");
			Description.SetDefault("Damage increased by 15%");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.allDamage += 0.15f;
		}
	}
}

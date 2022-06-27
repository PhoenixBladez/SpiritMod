using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
	public class BismitePotionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxin Strike");
			Description.SetDefault("Critical strikes inflict Festering Wounds\n4% increased critical strike chance");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.poisonPotion = true;
			player.GetCritChance(DamageClass.Generic) += 4;
		}
	}
}

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	public class StellarMinionBonus : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Empowerment");
			Description.SetDefault("Increases minion damage and knockback");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.minionDamage += .1f;
			player.minionKB += .05f;
		}
	}
}

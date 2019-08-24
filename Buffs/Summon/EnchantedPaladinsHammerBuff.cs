using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class EnchantedPaladinsHammerBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Enchanted Paladin's Hammer Minion");
			Description.SetDefault("A Enchanted Paladin's Hammer will fight for you.");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
			if (player.ownedProjectileCounts[mod.ProjectileType("EnchantedPaladinsHammerMinion")] > 0)
			{
				modPlayer.EnchantedPaladinsHammerMinion = true;
			}
			if (!modPlayer.EnchantedPaladinsHammerMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	class OlympiumGlobalItem : GlobalItem
	{
		public override bool? UseItem(Item item, Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (item.healLife > 0 && player.GetModPlayer<OlympiumPlayer>().eleutherios)
			{
				int healLife = item.healLife;
				PlayerLoader.GetHealLife(player, item, false, ref healLife);

				player.AddBuff(ModContent.BuffType<EleutheriosBuff>(), (int)(healLife / 5f) * 60);
			}
			return false;
		}
	}
}

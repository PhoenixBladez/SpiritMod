
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class DawnStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dawn Stone");
			Tooltip.SetDefault("8% increased melee damage\nIncreases melee critical strike chance the less health you have\nMelee attacks may burn enemies with solar rays, slightly reducing defense");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 8, 0, 0);
			item.rare = 3;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.08f;
			float defBoost = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 10f;
			player.meleeCrit += (int)defBoost;

			player.GetSpiritPlayer().sunStone = true;
		}

	}
}

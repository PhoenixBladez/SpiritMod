using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class TimScroll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tim's Scroll of Fumbling");
			Tooltip.SetDefault("'Who knew Skeletons could write?'\nMagic attacks may inflict random debuffs on foes\nMagic attacks may shoot out a random projectile");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().timScroll = true;
		}

	}
}

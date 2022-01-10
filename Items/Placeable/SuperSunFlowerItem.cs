using SpiritMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
	public class SuperSunFlowerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Sunflower");
			Tooltip.SetDefault("Places a large sunflower able to halt the spread of evil and Hallow" +
				"\nThis effect extends horizontally in a 5 block radius around the flower, and has an infinite vertical reach");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.consumable = true;
			item.createTile = ModContent.TileType<SuperSunFlower>();
			item.width = 36;
			item.height = 36;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(silver: 80);
		}
	}
}

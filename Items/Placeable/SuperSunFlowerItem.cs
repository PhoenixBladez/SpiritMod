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
			DisplayName.SetDefault("Pureglow");
			Tooltip.SetDefault("Places a large plant able to halt the spread of evil and Hallow" +
				"\nThis effect extends horizontally in a 5 block radius around the flower, and has an infinite vertical reach");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SuperSunFlower>();
			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 80);
		}
	}
}

using SpiritMod.Tiles.Ambient.Briar;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class BriarFlowerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowflower");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 5, 10);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<BriarBigFlowerUnnatural>();
		}
	}
}
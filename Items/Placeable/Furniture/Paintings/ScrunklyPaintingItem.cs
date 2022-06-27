using SpiritMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Paintings
{
	public class ScrunklyPaintingItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Scrunkly");
			Tooltip.SetDefault("'B. F. Jinx'");
		}

		public override void SetDefaults()
		{
			Item.Size = new Microsoft.Xna.Framework.Vector2(48);
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<ScrunklyPainting>();
		}
	}
}
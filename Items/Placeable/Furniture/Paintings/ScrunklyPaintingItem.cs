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
			item.Size = new Microsoft.Xna.Framework.Vector2(48);
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ScrunklyPainting>();
		}
	}
}
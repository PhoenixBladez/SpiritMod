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
			item.width = 22;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 5, 10);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<BriarBigFlowerUnnatural>();
		}
	}
}
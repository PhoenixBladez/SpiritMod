using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class Canvas : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blank Canvas");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = 1;
			item.maxStack = 99;
			item.createTile = ModContent.TileType<Tiles.Ambient.Canvas_Tile>();
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
	}
}
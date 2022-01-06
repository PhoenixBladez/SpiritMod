using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Tiles.Furniture.JadeDragonStatue
{
	public class DragonStatueItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon Statuette");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.value = Item.buyPrice(gold: 1);

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.rare = ItemRarityID.Blue;

			item.createTile = ModContent.TileType<DragonStatue>();
		}
	}
}
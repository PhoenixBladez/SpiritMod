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
			Item.width = 32;
			Item.height = 34;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.value = Item.buyPrice(gold: 1);

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.rare = ItemRarityID.Blue;

			Item.createTile = ModContent.TileType<DragonStatue>();
		}
	}
}
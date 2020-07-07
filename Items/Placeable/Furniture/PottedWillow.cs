using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PottedWillow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Willow Bonsai");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = Terraria.Item.buyPrice(0, 0, 80, 0);

            item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = mod.TileType("PottedWillowTile");
		}
	}
}